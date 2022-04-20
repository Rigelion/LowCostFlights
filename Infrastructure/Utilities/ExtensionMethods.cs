using Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities;

internal static class ExtensionMethods
{
    const string EUR_CURRENCY_CODE = "EUR";

    /// <summary>
    /// https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode=SYD&destinationLocationCode=BKK&departureDate=2022-11-01&adults=1&nonStop=false&max=250
    /// </summary>
    /// <param name="request"></param>
    /// <param name="origin">originLocationCode</param>
    /// <param name="destination">destinationLocationCode</param>
    /// <param name="departureDate">departureDate</param>
    /// <param name="numberOfPassengers">adults</param>
    /// <param name="returnDate">returnDate</param>
    /// <returns>RestRequest</returns>
    internal static RestRequest AddCheapFlightParameters(this RestRequest request, string origin, string destination, DateTime departureDate, uint numberOfPassengers, DateTime? returnDate = null)
    {
        request.AddParameter("currencyCode", EUR_CURRENCY_CODE);
        request.AddParameter("originLocationCode", origin);
        request.AddParameter("destinationLocationCode", destination);
        request.AddParameter("departureDate", departureDate.ToString("yyyy-MM-dd"));
        if (returnDate != null)
            request.AddParameter("returnDate", returnDate?.ToString("yyyy-MM-dd"));
        request.AddParameter("adults", numberOfPassengers);

        return request;
    }


    /// <summary>
    /// https://free.currconv.com/api/v7/convert?q=EUR_HRK&compact=ultra&apiKey=xxxxxxxxxxxxx
    /// EUR is used as primary currency
    /// HRK and USD are added as requested currencies
    /// </summary>
    /// <returns>RestRequest</returns>
    internal static RestRequest AddCurrencyConversionParameters(this RestRequest request, string apiKey)
    {
        request.AddParameter("q", $"EUR_HRK,EUR_USD");

        request.AddParameter("compact", "ultra");
        request.AddParameter("apiKey", apiKey);

        return request;
    }
}

