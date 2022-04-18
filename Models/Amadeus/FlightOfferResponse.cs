using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Amadeus;

public class FlightOfferResponse
{
    [JsonProperty("data")]
    public IEnumerable<FlightOfferData> ReturnedFlights { get; set; } = null!;
    [JsonProperty("dictionaries")]
    public Dictionaries Dictionaries { get; set; } = null!;
}

public class FlightOfferData
{
    [JsonProperty("type")]
    public string Type { get; set; } = null!;
    [JsonProperty("oneWay")]
    public string OneWay { get; set; } = null!;
    [JsonProperty("source")]
    public string Source { get; set; } = null!;
    [JsonProperty("numberOfBookableSeats")]
    public int NumberOfBookableSeats { get; set; }
    public IEnumerable<Itinerary> Itineraries { get; set; } = null!;
    [JsonProperty("price")]
    public FlightOfferPrice Price { get; set; } = null!;

}

public class FlightOfferPrice
{
    [JsonProperty("currency")]
    public string Currency { get; set; } = null!;
    [JsonProperty("total")]
    public decimal Total { get; set; }
}

public class Itinerary
{
    [JsonProperty("duration")]
    public string Duration { get; set; } = null!;

    public IEnumerable<Segment> Segments { get; set; } = null!;
}
public class Segment
{
    [JsonProperty("numberOfStops")]
    public int NumberOfStops { get; set; }
    [JsonProperty("carrierCode")]
    public string CarrierCode { get; set; } = null!;
    [JsonProperty("departure")]
    public FlightEndPoint Departure { get; set; } = null!;
    [JsonProperty("arrival")]
    public FlightEndPoint Arrival { get; set; } = null!;
    [JsonProperty("duration")]
    public string Duration { get; set; } = null!;
}

public class FlightEndPoint
{
    [JsonProperty("iataCode")]
    public string IataCode { get; set; } = null!;
}
public class Dictionaries
{
    [JsonProperty("carriers")]
    public IDictionary<string, string> Carriers { get; set; } = null!;
    [JsonProperty("aircraft")]
    public IDictionary<string, string> Aircrafts { get; set; } = null!;
}

