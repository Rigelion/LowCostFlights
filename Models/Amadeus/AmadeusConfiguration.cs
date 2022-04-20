using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Amadeus;

public class AmadeusConfiguration
{
    public string ApiKey { get; set; } = null!;
    public string ApiSecret { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
}

