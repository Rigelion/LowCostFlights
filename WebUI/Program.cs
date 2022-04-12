using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models.Amadeus;
using WebUI.Data;
using WebUI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IIataService, IataService>();
builder.Services.AddSingleton<AmadeusConfiguration>((serviceProvider) => builder.Configuration.GetSection(nameof(AmadeusConfiguration)).Get<AmadeusConfiguration>());
builder.Services.AddScoped<FlightOfferViewModel>();
builder.Services.AddScoped<IAmadeusService, AmadeusService>();
builder.Services.AddTransient<IValidator<FlightOfferViewModel>, FlightRequestViewModelValidator>();


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
