using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XAML_Projektarbete.Models;

namespace XAML_Projektarbete.DataProvider
{
    class CurrencyDataProvider
    {
        public static string ApiKey { get; set; }
        public async Task<Dictionary<string, Currency>> GetCurrencies()
        {
            string URL = $"https://free.currconv.com/api/v7/currencies?apiKey={ApiKey}";
            Dictionary<string, Currency> currencyDictionary = new Dictionary<string, Currency>();
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<CurrenciesRoot>(result.Result);
                    currencyDictionary = data.CurrencyResults;
                }
            }
            return currencyDictionary;
        }
    }
}
