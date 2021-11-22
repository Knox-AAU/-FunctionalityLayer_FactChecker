using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FactChecker.APIs
{

    public class WordRatio
    {
        public int articleId;
        public string word;
        public int count;
        public string title;
        public string filePath;
        public int totalWords;
        public string publisherName;
        public float percent;
    }
}
