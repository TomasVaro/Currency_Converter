using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XAML_Projektarbete.Models;

namespace XAML_Projektarbete.DataProvider
{
    public class CountryDataProvider
    {
        public static string ApiKey { get; set; }
        public async Task<Dictionary<string, Countries>> getAllCountriesUrl()
        {
            string URL = $"https://free.currconv.com/api/v7/countries?apiKey={ApiKey}";
            Dictionary<string, Countries> countriesDictionary = new Dictionary<string, Countries>();
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<CountriesRoot>(result.Result);
                    countriesDictionary = data.CurrencyResults;
                }
            }
            return countriesDictionary;
        }
    }
}
