using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Models.Amadeus;
using Models.CurrencyConverter;
using WebUI.ViewModels;

namespace WebUI.Utility;

public static class ExtensionMethods
{
    public static WebApplicationBuilder? AddServices(this WebApplicationBuilder builder, CurrencyRates currencyRates)
    {
        builder.Services.AddSingleton(currencyRates);
        builder.Services.AddSingleton<AmadeusConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(AmadeusConfiguration)).Get<AmadeusConfiguration>());
        builder.Services.AddSingleton<IIataService, IataService>();

        builder.Services.AddScoped<FlightOfferViewModel>();
        builder.Services.AddScoped<IAmadeusService, AmadeusService>();
        builder.Services.AddScoped<ICacheService, CacheService>();

        builder.Services.AddTransient<IValidator<FlightOfferViewModel>, FlightRequestViewModelValidator>();

        return builder;
    }

    public static async Task<CurrencyRates> GetCurrencyRatesAsync(this WebApplicationBuilder builder)
    {
        var currencyService = new CurrencyConverterService(builder.Configuration.GetSection(nameof(CurrencyConverterConfiguration)).Get<CurrencyConverterConfiguration>());
        CurrencyRates? currencyRates = await currencyService.GetCurrencyRates();

        if (currencyRates == null)
        {
            currencyRates = builder.Configuration.GetSection(nameof(CurrencyRates)).Get<CurrencyRates>();
        }

        return currencyRates;
    }
}

