using System.Net.Http;

namespace XAML_Projektarbete.DataProvider
{
    class APIHelper
    {
        public static HttpClient ApiClient { get; set; }
        public static void InitilizedClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
