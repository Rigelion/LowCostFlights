using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Amadeus;

public class FlightOfferRequest
{
    public FlightOfferRequest(string originIata, string destinationIata, DateTime departureDate, uint numberOfPassengers, DateTime? returnDate)
    {
        OriginIata = originIata;
        DestinationIata = destinationIata;
        DepartureDate = departureDate;
        NumberOfPassengers = numberOfPassengers;
        ReturnDate = returnDate;
    }

    public string OriginIata { get; set; }
    public string DestinationIata { get; set;}
    public DateTime DepartureDate { get; set; }
    public uint NumberOfPassengers { get; set; }
    public DateTime? ReturnDate { get; set; }
}

