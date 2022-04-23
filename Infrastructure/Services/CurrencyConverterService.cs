using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Models.CurrencyConverter;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class CurrencyConverterService : ICurrencyConverterService
{

    private readonly CurrencyRates DEFAULT_CURRENCY_RATES = new() { EurToHrkRate = 7.56m, EurToUsdRate = 1.07m };


    private readonly CurrencyConverterConfiguration _currencyConverterConfiguration;
    private readonly RestClient _restClient;
    private readonly ILogger<CurrencyConverterService> _logger;
    private readonly ICacheService _cacheService;

    public CurrencyConverterService(CurrencyConverterConfiguration currencyConverterConfiguration, ILogger<CurrencyConverterService> logger, ICacheService cacheService)
    {
        _currencyConverterConfiguration = currencyConverterConfiguration;
        _restClient = new(_currencyConverterConfiguration.BaseUrl);
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<CurrencyRates> GetCurrencyRates()
    {
        return await _cacheService.GetFromCacheAndCache(nameof(CurrencyRates), () => RequestCurrencyRates());
    }

    private async Task<CurrencyRates> RequestCurrencyRates()
    {
        var request = new RestRequest("convert", Method.Get);

        request.AddCurrencyConversionParameters(_currencyConverterConfiguration.ApiKey);

        var response = await _restClient.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            _logger.LogError("Currency Rates API response was not successful! {StatusCode} {@ErrorException}", response.StatusCode, response.ErrorException);
            _logger.LogInformation("Returning default currency rates {@CurrencyRates}", DEFAULT_CURRENCY_RATES);
            return DEFAULT_CURRENCY_RATES;
        }

        return JsonConvert.DeserializeObject<CurrencyRates>(response.Content!)!;
    }

}