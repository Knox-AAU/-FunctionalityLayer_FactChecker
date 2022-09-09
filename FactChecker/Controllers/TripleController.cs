    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FactChecker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public static TestData.WikiDataTriples WikiDataTriples = new ();

        [HttpGet]
        public IEnumerable<APIs.KnowledgeGraphAPI.KnowledgeGraphItem> Get()
        {
            return WikiDataTriples.triples;
        }

        [HttpPost]
        public APIs.KnowledgeGraphAPI.KnowledgeGraphItem Post(APIs.KnowledgeGraphAPI.KnowledgeGraphItem item)
        {
            TFIDF.TFIDFHandler tFIDFHandler = new ();
            string search = item.s + " " + item.r + " " + item.t;
            List<string> searchList = new List<string>();
            foreach(string s in search.Split(' '))
                searchList.Add(s);
            List<TFIDF.TFIDFItem> tFIDFItems = tFIDFHandler.CalculateTFIDF(searchList);
            List<int> articles = new List<int>();
            foreach(TFIDF.TFIDFItem article in tFIDFItems)
                articles.Add(article.articleId);
            TMWIIS.TMWIISHandler tMWIISHandler = new(articles, item);
            item.passage = tMWIISHandler.Evidence().ToString(); // This needs to be changed
            return item;
        }
    }
}
