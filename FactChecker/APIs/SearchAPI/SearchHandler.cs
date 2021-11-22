using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FactChecker.APIs
{
    public static class SearchHandler
    {

        //Search API
        public static string wordCountURL = "http://knox-node02.srv.aau.dk/wordcount/";
        public static string wordRatioURL = "http://localhost:5000/WordRatio";

        //Lemmatizer
        public static string lemmatizerURL = "http://knox-master01.srv.aau.dk/lemmatizer";


        static HttpClient client = new HttpClient();


        public static async Task<WordRatio[]> GetWordRatio (string term)
        {
            WordRatio[] articles = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(wordRatioURL + "?terms=" + term);
                if (response.IsSuccessStatusCode)
                {
                    articles = await response.Content.ReadAsAsync<WordRatio[]>();
                }
            }
            catch
            {
                throw;
            }
            return articles;
        }
    }
}
