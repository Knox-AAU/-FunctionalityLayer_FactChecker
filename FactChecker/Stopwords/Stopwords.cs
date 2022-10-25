using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.Stopwords
{
    public enum Stopwords_Language{
        da = 0,
        en = 1
    }
    public class Stopwords
    {
        public Dictionary<string,string> stopwords = new();
        public HashSet<string> stopwords_hashset = new();
        private Stopwords_Language stopwords_language {get; set;}
        public Stopwords (Stopwords_Language language = Stopwords_Language.en)
        {
            stopwords_language = language;
            GetStopWords();
        }

        public async void GetStopWords()
        {
            List<string> words = new();
            try
            {

            IO.FileStreamHandler fileStreamHandler = new();
            if(stopwords_language == Stopwords_Language.en){

                words = await fileStreamHandler.ReadFile("./TestData/stopwords.txt");
            } else {
                words = await fileStreamHandler.ReadFile("./TestData/danish_stopwords.txt");

            }
            
            foreach(string s in words)
            {
                stopwords_hashset.Add(s);
                if (!stopwords.ContainsKey(s))
                {
                    stopwords.Add(s, s);
                }
            }
            } catch (Exception)
            {
                stopwords = new() { };
            }
        }

    }
}
