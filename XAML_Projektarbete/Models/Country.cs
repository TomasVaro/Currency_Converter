using Newtonsoft.Json;
using System.Collections.Generic;

namespace XAML_Projektarbete.Models
{
    public class CountriesRoot
    {
        [JsonProperty("results")]
        public Dictionary<string, Countries> CurrencyResults { get; set; }
    }

    public class Countries
    {
        [JsonProperty("alpha3")]
        public string Alpha3 { get; set; }

        [JsonProperty("currencyId")]
        public string CurrencyId { get; set; }

        [JsonProperty("currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
