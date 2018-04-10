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
namespace WpfApplication1
{
    class StockApi
    {


        public class DataObject
        {
            public string Name { get; set; }
        }

        public StockApi(string URL, string urlParameters)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                HttpContent requestContent = response.Content;
                string json = requestContent.ReadAsStringAsync().Result;
                //dynamic j = JObject.Parse(json);
                var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
                MessageBox.Show(values["Meta Data"]["1. Information"].ToString());
                
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
