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
using F23.StringSimilarity;
using FactChecker.APIs.LemmatizerAPI;

namespace FactChecker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public static TestData.WikiDataTriples WikiDataTriples = new ();
        readonly IEvidenceRetrieval er = new TMWIIS.TMWIISHandler();
        readonly IArticleRetrieval ar = new TFIDF.TFIDFHandler();
        readonly IPassageRetrieval pr = new PassageRetrieval.PassageRetrievalHandler();
        readonly LemmatizerHandler lh = new();
        readonly Jaccard js = new();
        
        [HttpGet]
        public IEnumerable<KnowledgeGraphItem> Get()
        {
            return WikiDataTriples.triples;
        }

        // Add support for multiple knowledgegraph items and sum the tf-idf score to get the result
        [HttpPost]
        public ActionResult<KnowledgeGraphItem> Post([FromBody]KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            item.passage = er.GetEvidence(articles, item).FirstOrDefault()?.Text ?? "No Passage found";
            return Ok(item);
        }

        [HttpPost("Jaccard")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostJaccard([FromBody] KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            IEnumerable<Passage> passages = er.GetEvidence(articles, item).Take(50);
            foreach (var p in passages)
            {
                //string triple = (await lh.GetLemmatizedText($"{item.s} {item.r} {item.t}", "en")).lemmatized_string;
                //string? passage_ = (await lh.GetLemmatizedText(p.Text, "en"))?.lemmatized_string ?? null;
                //if (passage_ == null) continue;
                p.Score = (float)Math.Round(js.Similarity($"{item.s} {item.r} {item.t}", p.Text), 2);
            }
            return Ok(passages.ToList().OrderByDescending(p => p.Score));
        }
        [HttpPost("Multiple")]
        public ActionResult<KnowledgeGraphItem> PostMultiple([FromBody]MultipleKnowledgeGraphItem Request)
        {
            List<Article> articles = ar.GetArticles(Request.items).ToList();
            Request.passage = er.GetEvidence(articles, Request.items).FirstOrDefault()?.Text ?? "No Passage found";
            return Ok(Request);
        }
    }
}