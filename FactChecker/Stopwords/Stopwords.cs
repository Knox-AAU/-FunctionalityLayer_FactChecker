using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.Stopwords
{
    public class Stopwords
    {
        public Dictionary<string,string> stopwords = new();
        public Stopwords ()
        {
            GetStopWords();
        }

        public async void GetStopWords()
        {
            IO.FileStreamHandler fileStreamHandler = new();
            List<string> words = await fileStreamHandler.ReadFile("./TestData/stopwords.txt");

            foreach(string s in words)
            {

                if (!stopwords.ContainsKey(s))
                {
                    stopwords.Add(s, s);
                }
            }
        }

    }
}
