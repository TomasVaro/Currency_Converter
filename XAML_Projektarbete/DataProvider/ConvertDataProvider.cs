using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using XAML_Projektarbete.Models;

namespace XAML_Projektarbete.DataProvider
{
    public class ConvertDataProvider
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

        public async Task<double> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            string URL = $"https://free.currconv.com/api/v7/convert?q={fromCurrency}_{toCurrency}&compact=ultra&apiKey={apiKey}";
            var exchangeRate = 0.00;
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(result);
                    exchangeRate = data.First.ToObject<double>();
                }
            }
            return exchangeRate;
        }

        public async Task<double> GetHistoricalExchangeRate(string fromCurrency, string toCurrency, string date)
        {
            string URL = $"https://free.currconv.com/api/v7/convert?apiKey=37caa54a777a956b193b&q={fromCurrency}_{toCurrency}&compact=ultra&date={date}";
            var exchangeRate = 0.00;
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(result);
                    exchangeRate = data.First.First.First.ToObject<double>();
                }
            }
            return exchangeRate;
        }
    }
}
