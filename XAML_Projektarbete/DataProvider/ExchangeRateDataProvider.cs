using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
    }
}
