using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XAML_Projektarbete.Models;

namespace XAML_Projektarbete.DataProvider
{
    class CurrencyDataProvider
    {
        private string apiKey = "37caa54a777a956b193b";
        public async Task<Dictionary<string, Currency>> GetCurrencies()
        {
            string URL = $"https://free.currconv.com/api/v7/currencies?apiKey={apiKey}";
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
