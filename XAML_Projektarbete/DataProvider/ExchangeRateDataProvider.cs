using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
    public class ExchangeRateDataProvider
    {
        private string apiKey = "37caa54a777a956b193b";
        
        public async Task<double> GetExchangeRate(string fromCurrency, string toCurrency, string date)
        {
            string URL = $"https://free.currconv.com/api/v7/convert?apiKey={apiKey}&q={fromCurrency}_{toCurrency}&compact=ultra&date={date}";
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

        public async Task<Dictionary<string, object>> GetHistoricalExchangeRates(string fromCurrency, string toCurrency, string fromDate, string endDate)
        {
            string URL = $"https://free.currconv.com/api/v7/convert?q={fromCurrency}_{toCurrency}&compact=ultra&apiKey={apiKey}";
            //var exchangeRates = 0.00;
            Dictionary<string, object> exchangeRatesDictionary = new Dictionary<string, object>();
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(result);
                    exchangeRatesDictionary = data.First.ToObject<Dictionary<string, Object>>();
                }
            }
            return exchangeRatesDictionary;
        }
    }
}
