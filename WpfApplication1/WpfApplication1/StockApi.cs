using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Headers;
using System.Diagnostics;
namespace WpfApplication1
{
    class StockApi
    {

        private static string URL = "https://www.alphavantage.co/query";
        //private static string URL = "http://192.168.0.24";

        private HttpClient client;

        public StockApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            
        }
        public Dictionary<string, dynamic> getData(string urlParameters)
        {
            
            HttpResponseMessage response = Task.Run(() => client.GetAsync(urlParameters)).Result;   // Blocking call!
         if (response.IsSuccessStatusCode)
            {
                
                // Parse the response body. Blocking!
                HttpContent responseContent = response.Content;
                string json = responseContent.ReadAsStringAsync().Result;
                var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
                client.Dispose();
                return values;
            }
            else
            {
                MessageBox.Show( ((int)response.StatusCode).ToString() + " " + response.ReasonPhrase);
                client.Dispose();
                return null;
            }
        }
    }
}
