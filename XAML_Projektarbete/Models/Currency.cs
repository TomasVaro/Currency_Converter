using Newtonsoft.Json;
using System.Collections.Generic;

namespace XAML_Projektarbete.Models
{
    public class CurrenciesRoot
    {
        [JsonProperty("results")]
        public Dictionary<string, Currency> CurrencyResults { get; set; }
    }
    public class Currency
    {
        [JsonProperty("currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

}
