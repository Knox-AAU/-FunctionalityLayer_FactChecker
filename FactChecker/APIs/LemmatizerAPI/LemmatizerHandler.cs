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
        readonly HttpClient __client;

        public LemmatizerHandler()
        {
            __client = new();
        }

        /// <summary>
        /// Method taking two parameters of type (<paramref name="string"/>, <paramref name="string"/>).
        /// Used to lemmatize some text in a specific language. 
        /// NOTE: In the current iteration, the lemmatizer is implemented on the server in Python.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="language"></param>
        /// <returns>A LemmatizerItem containing the lemmatized string</returns>
        public async Task<string> GetLemmatizedText(string text, string? language = null)
        {
            string data;
            if (language == null)
                data = "{\"string\":\"" + text + "\"}";
            else
                data = "{\"string\":\"" + text + "\"," +
                           "\"language\":\"" + language + "\"}";
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            LemmatizerItem lemmatizerItem;
            HttpResponseMessage response = await __client.PostAsync(lemmatizerURL, content);
            response.EnsureSuccessStatusCode();
            lemmatizerItem = await response.Content.ReadAsAsync<LemmatizerItem>();
            return lemmatizerItem.lemmatized_string;
        }
        public async Task<string> GetLanguageFromText(string text)
        {
            string data = "{\"string\":\"" + text + "\"}";
            
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            LemmatizerItem lemmatizerItem;
            HttpResponseMessage response = await __client.PostAsync($"{lemmatizerURL}GetLanguage", content);
            response.EnsureSuccessStatusCode();
            lemmatizerItem = await response.Content.ReadAsAsync<LemmatizerItem>();
            return lemmatizerItem.lemmatized_language;
        }
    }
}
