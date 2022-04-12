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

    private string _accessToken = null!;

    public AmadeusService(AmadeusConfiguration amadeusConfiguration, ILogger<AmadeusService> logger)
    {
        _amadeusConfiguration = amadeusConfiguration;
        _logger = logger;
        _restClient = new(_amadeusConfiguration.BaseUrl);
    }

    public async Task<FlightOfferResponse?> GetFlightOffers(IataModel originIata, IataModel destinationIata, DateTime departureDate, uint numberOfPassengers, DateTime? returnDate = null)
    {
        var request = new RestRequest(@"/v2/shopping/flight-offers", Method.Get);
        var policy = SetRetryPolicy();

        _ = DateTime.TryParse("2022-11-01", out var date);

        request.AddCheapFlightParameters(new() { Iata = "SYD" }, new() { Iata = "BKK" }, date, 1);

        //request.AddCheapFlightParameters(originIata, destinationIata, departureDate, numberOfPassengers, returnDate);

        var response = await policy.ExecuteAsync(context =>
        {
            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            return _restClient.ExecuteAsync(request);
        }, CancellationToken.None);

        return JsonConvert.DeserializeObject<FlightOfferResponse>(response.Content!)!;
    }

    private AsyncRetryPolicy<RestResponse> SetRetryPolicy()
    {
        var policy = Policy.HandleResult<RestResponse>(x => x.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            .RetryAsync(1, async (result, retryCount, context) =>
            {
                await GetNewAccessToken();
            }

            );
        return policy;
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
}

