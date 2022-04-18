using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Models.Amadeus;
using Models.CurrencyConverter;
using WebUI.Data;
using WebUI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Get currency rates
var currencyService = new CurrencyConverterService(builder.Configuration.GetSection(nameof(CurrencyConverterConfiguration)).Get<CurrencyConverterConfiguration>());

var currencyRates = await currencyService.GetCurrencyRates();

builder.Services.AddSingleton(currencyRates);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<AmadeusConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(AmadeusConfiguration)).Get<AmadeusConfiguration>());
//builder.Services.AddSingleton<CurrencyConverterConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(CurrencyConverterConfiguration)).Get<CurrencyConverterConfiguration>());

builder.Services.AddSingleton<IIataService, IataService>();

builder.Services.AddScoped<FlightOfferViewModel>();
builder.Services.AddScoped<IAmadeusService, AmadeusService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddTransient<IValidator<FlightOfferViewModel>, FlightRequestViewModelValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
