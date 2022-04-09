using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class IataModel
{
    [JsonProperty("code")]
    public string Iata { get; set; } = null!;
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("city")]
    public string City { get; set; } = null!;
    [JsonProperty("state")]
    public string State { get; set; } = null!;
    [JsonProperty("country")]
    public string Country { get; set; } = null!;

}

