using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace XAML_Projektarbete.DataProvider
{
    class HistoricalExchangeRateDataProvider
    {
        public static string ApiKey { get; set; }
        public async Task<Dictionary<string, double>> GetHistoricalExchangeRates(string fromCurrency, string toCurrency, string fromDate, string endDate)
        {
            string URL = $"https://free.currconv.com/api/v7/convert?apiKey={ApiKey}&q={fromCurrency}_{toCurrency}&compact=ultra&date={fromDate}&endDate={endDate}";
            Dictionary<string, double> exchangeRatesDictionary = new Dictionary<string, double>();
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(URL))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(result);
                    exchangeRatesDictionary = data.First().Value;
                }
            }
            return exchangeRatesDictionary;
        }
    }
}
