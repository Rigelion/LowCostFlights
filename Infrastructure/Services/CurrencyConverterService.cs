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

public class CurrencyConverterService
{
    private readonly CurrencyConverterConfiguration _currencyConverterConfiguration;
    private readonly RestClient _restClient;

    public CurrencyConverterService(CurrencyConverterConfiguration currencyConverterConfiguration)
    {
        _currencyConverterConfiguration = currencyConverterConfiguration;
        _restClient = new(_currencyConverterConfiguration.BaseUrl);
    }

    public async Task<CurrencyRates> GetCurrencyRates()
    {
        var request = new RestRequest("convert", Method.Get);

        request.AddCurrencyConversionParameters(_currencyConverterConfiguration.ApiKey);
        var response = await _restClient.ExecuteAsync(request);

         return JsonConvert.DeserializeObject<CurrencyRates>(response.Content!)!;
    }

}