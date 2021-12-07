using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace FactChecker.APIs.LemmatizerAPI
{
    /// <summary>
    /// A class containing the methods GetLemmatizedText(string, language) and GetLemmatizedText(string).
    /// Used to lemmatize some text in a specific or detected language.
    /// </summary>
    public class LemmatizerHandler
    {
        public string lemmatizerURL = "http://localhost:5000/";
        HttpClient client = new HttpClient();

        /// <summary>
        /// Method taking two parameters of type (<paramref name="string"/>, <paramref name="string"/>).
        /// Used to lemmatize some text in a specific language. 
        /// NOTE: In the current iteration, the lemmatizer is implemented on the server in Python.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="language"></param>
        /// <returns>A LemmatizerItem containing the lemmatized string</returns>
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

        /// <summary>
        /// Method taking one parameter of type (<paramref name="string"/>).
        /// Used to lemmatize some text using language detection. 
        /// NOTE: In the current iteration, the lemmatizer is implemented on the server in Python.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="language"></param>
        /// <returns>A LemmatizerItem containing the lemmatized string</returns>
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
