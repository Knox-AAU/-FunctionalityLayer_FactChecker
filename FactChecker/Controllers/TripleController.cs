using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Intefaces;
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
        public IEnumerable<KnowledgeGraphItem> Get()
        {
            return WikiDataTriples.triples;
        }

        // Add support for multiple knowledgegraph items and sum the tf-idf score to get the result
        [HttpPost]
        public ActionResult<KnowledgeGraphItem> Post(KnowledgeGraphItem item)
        {
            List<Article> articles = new TFIDF.TFIDFHandler().GetArticles(item).ToList();
            item.passage = new TMWIIS.TMWIISHandler().GetEvidence(articles, item).FirstOrDefault()?.Text ?? "No Passage found";
            return Ok(item);
        }
    }
}