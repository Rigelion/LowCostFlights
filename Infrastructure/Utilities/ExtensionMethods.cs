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
    internal static  RestRequest AddCheapFlightParameters(this RestRequest request, IataModel origin, IataModel destination, DateTime departureDate, int numberOfPassengers, DateTime? returnDate = null)
    {
        request.AddParameter("originLocationCode", origin.Iata);
        request.AddParameter("destinationLocationCode", destination.Iata);
        request.AddParameter("departureDate", departureDate.ToString("yyyy-MM-dd"));
        if (returnDate != null)
            request.AddParameter("returnDate", returnDate?.ToString("yyyy-MM-dd"));
        request.AddParameter("adults", numberOfPassengers);

        return request;
    }

}

