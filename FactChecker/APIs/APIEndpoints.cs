using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FactChecker.APIs
{
    public static class APIEndpoints
    {

        //Search API
        public static string wordCountURL = "http://knox-node02.srv.aau.dk/wordcount/";
        public static string wordRatioURL = "http://knox-node02.srv.aau.dk/wordratio/";

        //Lemmatizer
        public static string lemmatizerURL = "http://knox-master01.srv.aau.dk/lemmatizer";


        static HttpClient client = new HttpClient();


        public static async Task<WordRatio> GetWordRatio (string[] terms)
        {
            WordRatio articles = null;
            HttpResponseMessage response = await client.GetAsync(APIEndpoints.wordCountURL);
            if (response.IsSuccessStatusCode)
            {
                articles = await response.Content.ReadAsAsync<WordRatio>();
            }

            return articles;
        }
    }
}
