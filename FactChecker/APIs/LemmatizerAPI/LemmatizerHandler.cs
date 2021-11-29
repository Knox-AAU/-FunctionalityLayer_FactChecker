using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace FactChecker.APIs.LemmatizerAPI
{
    public class LemmatizerHandler
    {
        public string lemmatizerURL = "http://localhost:5000/";
        HttpClient client = new HttpClient();

        public async Task<LemmatizerItem> GetLemmatizedText(string text, string language)
        {
            string data = "{\"string\":\"" + text + "\"," +
                           "\"language\":\"" + language + "\"}";
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
      
            LemmatizerItem lemmatizerItem = null;
            try
            {
                HttpResponseMessage response = await client.PostAsync(lemmatizerURL, content);
                Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    lemmatizerItem = await response.Content.ReadAsAsync<LemmatizerItem>();
                }
            }
            catch
            {
                throw;
            }
            return lemmatizerItem;
        }

        public async Task<LemmatizerItem> GetLemmatizedText(string text)
        {
            string data = "{\"string\":\"" + text + "\"}";
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            LemmatizerItem lemmatizerItem = null;
            try
            {
                HttpResponseMessage response = await client.PostAsync(lemmatizerURL, content);
                Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    lemmatizerItem = await response.Content.ReadAsAsync<LemmatizerItem>();
                }
            }
            catch
            {
                throw;
            }
            return lemmatizerItem;
        }
    }
}
