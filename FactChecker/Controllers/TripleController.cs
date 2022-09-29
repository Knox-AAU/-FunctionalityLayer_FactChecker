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
        public static TestData.WikiDataTriples WikiDataTriples = new();
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
        public ActionResult<KnowledgeGraphItem> Post([FromBody] KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            item.passage = er.GetEvidence(articles, item).FirstOrDefault()?.FullPassage ?? "No Passage found";
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
                p.Score = (float)Math.Round(js.Similarity($"{item.s} {item.r} {item.t}", p.FullPassage), 2);
            }
            return Ok(passages.ToList().OrderByDescending(p => p.Score));
        }

        [HttpPost("JaccardLevenshtein")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostLeven([FromBody] KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            IEnumerable<Passage> passages = er.GetEvidence(articles, item).Take(50);
            foreach (var p in passages)
            {
                //string triple = (await lh.GetLemmatizedText($"{item.s} {item.r} {item.t}", "en")).lemmatized_string;
                //string? passage_ = (await lh.GetLemmatizedText(p.Text, "en"))?.lemmatized_string ?? null;
                //if (passage_ == null) continue;
                p.ls_score = (double)Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance_V2($"{item.s} {item.r} {item.t}", p.FullPassage) / p.FullPassage.Length * 100;
                p.js_score = Math.Round(js.Similarity($"{item.s} {item.r} {item.t}", p.FullPassage), 2) * 2000;
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
        public ActionResult<KnowledgeGraphItem> PostMultiple([FromBody] KnowledgeGraphItem Request)
        {
            List<Article> articles = ar.GetArticles(Request).ToList();
            Request.passage = er.GetEvidence(articles, Request).FirstOrDefault()?.FullPassage ?? "No Passage found";
            return Ok(Request);
        }
        [HttpPost("Rake")]
        public async Task<ActionResult<List<Passage>>> PostRake([FromBody] KnowledgeGraphItem Request)
        {

            List<Article> articles = ar.GetArticles(Request).ToList();
            List<Passage> passages = new();
            List<Passage> tmp = new List<Passage>();
            Rake.Rake r = new();
            foreach (var article in articles)
            {
                tmp = r.GetPassages(article).ToList();
                foreach (var passage in tmp)
                {
                    passage.Artical_ID = article.Id;
                    passages.Add(passage);
                }
            }
            foreach (var p in passages)
            {
                p.ls_score = (double)Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance_V2($"{Request.s} {Request.r} {Request.t}", p.FullPassage) / p.FullPassage.Length * 100;
                p.js_score = Math.Round(js.Similarity($"{Request.s} {Request.r} {Request.t}", p.FullPassage), 2) * 2000;
            }
            passages = passages.OrderBy(p => p.ls_score).ToList();
            for (int i = 0; i < passages.Count(); i++)
            {
                Passage passage = passages.ToList()[i];
                passage.ls_rank = i + 1;
            }
            passages = passages.OrderByDescending(p => p.js_score).ToList();
            for (int i = 0; i < passages.Count(); i++)
            {
                Passage passage = passages.ToList()[i];
                passage.js_rank = i + 1;
            }
            foreach (var item_ in passages)
            {
                item_.Score = item_.js_rank + item_.ls_rank;
            }
            passages = passages.OrderBy(p => p.Score).ToList();
            return Ok(passages);
        }

        [HttpPost("TF-IDF")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostTfIdRewrite([FromBody] MultipleKnowledgeGraphItem item)
        {
            FactChecker.PassageRetrieval.ArticleRetrievalHandlerV2 arv2 = new();
            List<Article> articles = arv2.GetArticles(item.items).ToList();
            foreach (var item2 in articles)
            {
                item2.FullText = "";
                item2.TFIDF *= 10000;
            }
            return Ok(articles);
        }
    }
}