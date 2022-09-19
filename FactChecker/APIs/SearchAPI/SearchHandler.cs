using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FactChecker.APIs
{
    /// <summary>
    /// A class containing the method GetSearchItem. Used for handling searches on the Knox server.
    /// NOTE: This class is NOT used in this iteration of Knox but might be useful in the future.
    /// </summary>
    public class SearchHandler
    {

        public string wordRatioURL = "http://knox-node02.srv.aau.dk/WordRatio";

        private readonly HttpClient _client = new();

        public async Task<SearchItem[]> GetSearchItem (string term)
        {
            HttpResponseMessage response = await _client.GetAsync(wordRatioURL + "?terms=" + term);
            response.EnsureSuccessStatusCode();
            var articles = await response.Content.ReadAsAsync<SearchItem[]>();
            return articles;
        }
    }
}
