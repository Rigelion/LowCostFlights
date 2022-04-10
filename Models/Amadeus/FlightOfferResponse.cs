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
    public IEnumerable<FlightOfferData> Data { get; set; } = null!;
}

public class FlightOfferData
{
    [JsonProperty("type")]
    public string Type { get; set; } = null!;
    [JsonProperty("oneWay")]
    public string OneWay { get; set; } = null!;
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

