using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Models.Amadeus;
using Models.CurrencyConverter;
using WebUI.ViewModels;

namespace WebUI.Utility;

public static class ExtensionMethods
{
    public static WebApplicationBuilder? AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<AmadeusConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(AmadeusConfiguration)).Get<AmadeusConfiguration>());
        builder.Services.AddSingleton<CurrencyConverterConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(CurrencyConverterConfiguration)).Get<CurrencyConverterConfiguration>());
        builder.Services.AddSingleton<IIataService, IataService>();

        builder.Services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
        builder.Services.AddScoped<FlightOfferViewModel>();
        builder.Services.AddScoped<IAmadeusService, AmadeusService>();
        builder.Services.AddScoped<ICacheService, CacheService>();

        builder.Services.AddTransient<IValidator<FlightOfferViewModel>, FlightRequestViewModelValidator>();

        return builder;
    }
}

