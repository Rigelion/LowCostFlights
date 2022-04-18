using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Models;
using Models.Amadeus;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class AmadeusService : IAmadeusService
{
    private readonly RestClient _restClient;
    private readonly AmadeusConfiguration _amadeusConfiguration;
    private readonly ILogger<AmadeusService> _logger;
    private readonly ICacheService _cacheService;

    private string _accessToken = null!;

    public AmadeusService(AmadeusConfiguration amadeusConfiguration, ILogger<AmadeusService> logger, ICacheService cacheService)
    {
        _amadeusConfiguration = amadeusConfiguration;
        _logger = logger;
        _restClient = new(_amadeusConfiguration.BaseUrl);
        _cacheService = cacheService;
    }

    public async Task<FlightOfferResponse?> GetFlightOffers(FlightOfferRequest request)
    {
        return await _cacheService.GetFromCacheAndCache(GetCacheKey(request), () => FlightOffersRequest(request));
    }

    private async Task<FlightOfferResponse?> FlightOffersRequest(FlightOfferRequest flightOfferRequest)
    {
        var restRequest = new RestRequest(@"/v2/shopping/flight-offers", Method.Get);

        var unauthorizedPolicy = Policy
            .HandleResult<RestResponse>(x => x.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            .RetryAsync(1, async (result, retryCount, context) =>
            {
                await GetNewAccessToken();
            });

        var policy = Policy
            .HandleResult<RestResponse>(x => x.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            .RetryAsync(5);

        _ = DateTime.TryParse("2022-11-01", out var date);

        restRequest.AddCheapFlightParameters(flightOfferRequest.OriginIata, flightOfferRequest.DestinationIata, flightOfferRequest.DepartureDate, flightOfferRequest.NumberOfPassengers, flightOfferRequest.ReturnDate);

        var response = await unauthorizedPolicy.WrapAsync(policy).ExecuteAsync(context =>
        {
            restRequest.AddHeader("Authorization", $"Bearer {_accessToken}");

            return _restClient.ExecuteAsync(restRequest);
        }, CancellationToken.None);

        return JsonConvert.DeserializeObject<FlightOfferResponse>(response.Content!)!;
    }

    private async Task GetNewAccessToken()
    {
        var url = @"/v1/security/oauth2/token";

        var request = new RestRequest(url, Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddBody($"grant_type=client_credentials&client_id={_amadeusConfiguration.ApiKey}&client_secret={_amadeusConfiguration.ApiSecret}", "application/x-www-form-urlencoded");

        var t = await _restClient.ExecutePostAsync(request);

        var x = JsonConvert.DeserializeObject<AmadeusAuthorizationResponse>(t.Content!);
        _accessToken = x!.AccessToken;
    }


    private string GetCacheKey(FlightOfferRequest request)
    {
        return $"{request.OriginIata}-{request.DestinationIata}-{request.DepartureDate}-{request.NumberOfPassengers}-{request.ReturnDate}";
    }
}

