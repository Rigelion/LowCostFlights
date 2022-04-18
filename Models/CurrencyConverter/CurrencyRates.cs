using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CurrencyConverter
{
    public class CurrencyRates
    {
        [JsonProperty("EUR_HRK")]
        public decimal EurToHrkRate { get; set; }
        [JsonProperty("EUR_USD")]
        public decimal EurToUsdRate { get; set; }
    }
}
