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
            // Add support for multiple knowledgegraph items and sum the tf-idf score to get the result
            TFIDF.TFIDFHandler tFIDFHandler = new ();
            List<string> searchList = new() { item.s, item.r, item.t };
            List<TFIDF.TFIDFItem> tFIDFItems = tFIDFHandler.CalculateTFIDF(searchList);
            List<int> articles = tFIDFItems.Select(p => p.articleId).ToList();
            TMWIIS.TMWIISHandler tMWIISHandler = new(articles, item);
            item.passage = tMWIISHandler.Evidence().OrderByDescending(p => p.score).FirstOrDefault()?.passage ?? "No Passage found";
            return item;
        }
    }
}
