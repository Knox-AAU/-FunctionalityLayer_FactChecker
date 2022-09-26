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

        [HttpPost("Leven")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostLeven([FromBody] KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            IEnumerable<Passage> passages = er.GetEvidence(articles, item).Take(50);
            foreach (var p in passages)
            {
                //string triple = (await lh.GetLemmatizedText($"{item.s} {item.r} {item.t}", "en")).lemmatized_string;
                //string? passage_ = (await lh.GetLemmatizedText(p.Text, "en"))?.lemmatized_string ?? null;
                //if (passage_ == null) continue;
                p.ls_score = (double)Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance($"{item.s} {item.r} {item.t}", p.Text) / (double)p.Text.Length * 100;
                p.js_score = Math.Round(js.Similarity($"{item.s} {item.r} {item.t}", p.Text), 2) * 2000;
            }
            passages = passages.ToList().OrderBy(p => p.ls_score);
            for (int i = 0; i < passages.Count(); i++)
            {
                Passage passage = passages.ToList()[i];
                passage.ls_rank = i + 1;
            }
            passages = passages.ToList().OrderByDescending(p => p.js_score);
            for (int i = 0; i < passages.Count(); i++)
            {
                Passage passage = passages.ToList()[i];
                passage.js_rank = i + 1;
            }
            foreach (var item_ in passages)
            {
                item_.Score = item_.js_rank + item_.ls_rank;
            }
            return Ok(passages.ToList().OrderBy(p => p.Score)); 
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