using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
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
using FactChecker.TFIDF;
using FactChecker.Levenshtein;
using static FactChecker.Controllers.AlgChooser;

namespace FactChecker.Controllers
{

    public class AlgChooserReturn
    {
        public float Confidence { get; set; } = 0;
        public List<Article> Articles { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public static TestData.WikiDataTriples WikiDataTriples = new();
        readonly IEvidenceRetrieval er = new TMWIIS.TMWIISHandler();
        readonly IArticleRetrieval ar = new TFIDF.TFIDFHandler();
        readonly IPassageRetrieval pr = new PassageRetrieval.PassageRetrievalHandler();
        readonly IPassageRetrieval rake = new Rake.Rake(sentences_min_length: 100*4);
        readonly SimRank.SimRank simRank = new();
        readonly LemmatizerHandler lh = new();
        readonly WordEmbedding.WordEmbedding wordEmbedding = new();
        readonly Jaccard js = new();
        SimRank.SimRank sr = new();

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


        [NonAction]
        public List<Passage> PassageExtraction(AlgChooser algs, Article art)
        {
            return algs.PassageExtraction switch
            {
                PassageExtractionEnum.Default => pr.GetPassages(art).Take(50).ToList(),
                PassageExtractionEnum.Rake => rake.GetPassages(art).Take(50).ToList(),
                _ => pr.GetPassages(art).ToList()
            };
        }
        [NonAction]
        public List<Article> ArticleRetrieval(AlgChooser algs)
        {
            return algs.ArticleRetrieval switch
            {
                ArticleRetrievalEnum.TF_IDF => ar.GetArticles(algs.MultipleKnowledgeGraphItem.Items).ToList(),
                _ => ar.GetArticles(algs.MultipleKnowledgeGraphItem.Items).ToList()
            };
        }

        #region Calculations
        [NonAction]
        public double CalculateJaccard(AlgChooser algs, Passage passage) 
            => (double)Math.Round(js.Similarity(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage), 2);
        [NonAction]
        public double CalculateLevenshtein(AlgChooser algs, Passage passage)
            => (double)LevenshteinDistanceAlgorithm.LevenshteinDistance_V2(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
        [NonAction]
        public double CalculateCosine(AlgChooser algs, Passage passage)
            => (double)new Cosine.CosineSim().similarity_v2(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
        [NonAction]
        public double CalculateWordEmbedding(AlgChooser algs, Passage passage)
            => wordEmbedding.GetEvidence(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
        [NonAction]
        public float CalculateConfidence(AlgChooser algs)
        {
            return (algs.ConfidenceEnum) switch
            {
                ConfidenceEnum.SimRank => MathF.Round(simRank.GetSimRank(algs.MultipleKnowledgeGraphItem), 2),
                _ => -1f
            };
        }
        #endregion

        [NonAction]
        public List<Passage> PassageRanking(AlgChooser algs, List<Passage> passages)
        {
            foreach (Passage passage in passages)
                foreach (var passageRanking in algs.PassageRankings)
                    switch (passageRanking)
                    {
                        case PassageRankingEnum.Jaccard:
                            passage.JaccardScore = CalculateJaccard(algs, passage);
                            break;
                        case PassageRankingEnum.Levenshtein:
                            passage.LevenshteinScore = CalculateLevenshtein(algs, passage);
                            break;
                        case PassageRankingEnum.Cosine:
                            passage.CosineScore = CalculateCosine(algs, passage);
                            break;
                        case PassageRankingEnum.WordEmbedding:
                            passage.WordEmbeddingScore = CalculateWordEmbedding(algs, passage);
                            break;
                        default:
                            break;
                    }
            foreach (var passageRanking in algs.PassageRankings)
                switch (passageRanking)
                {
                    case PassageRankingEnum.Jaccard:
                        passages.RankJaccard();
                        break;
                    case PassageRankingEnum.Cosine:
                        passages.RankCosine();
                        break;
                    case PassageRankingEnum.WordEmbedding:
                        passages.RankWordEmbedding();
                        break;
                    case PassageRankingEnum.Levenshtein:
                        passages.RankWordLevenshtein();
                        break;
                }
            return passages;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="algs"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// 
        ///     {
            ///    "PassageExtraction": 1,
            ///    "ArticleRetrieval": 0,
            ///    "PassageRankings": [0, 1, 2, 3],
            ///    "ConfidenceEnum": 1,
            ///    "MultipleKnowledgeGraphItem": {
            ///        "Items": [
            ///            {
            ///                "s": "Holland",
            ///                "r": "instance of",
            ///                "t": "Spider-Man"
            ///            },
            ///           {
            ///              "s": "Tom Holland",
            ///                "r": "instance of",
            ///                "t": "Spider-Man"
            ///            }
            ///        ]
            ///    }
            ///}
        /// 
        /// </remarks>
        [HttpPost("AlgChooser")]
        [ProducesResponseType(typeof(AlgChooserReturn), 200)]
        [Produces("application/json")]
        public async Task<ActionResult<List<Article>>> PostAlgChooser([FromBody] AlgChooser algs)
        {
            List<Article> articles = ArticleRetrieval(algs);
            foreach (var art in articles)
            {
                art.Passages = PassageExtraction(algs, art);
                art.Passages = PassageRanking(algs, art.Passages);
                foreach (var passage in art.Passages) passage.CalculateScoreFromKeyValuePairs();
                art.Passages = art.Passages.OrderBy(p => p.Score).ToList();
                art.FullText = art.FullText[0..100];
                art.Passages = art.Passages.Take(3).ToList();
            }
            return Ok(
                new AlgChooserReturn()
                {
                    Articles = articles,
                    Confidence = CalculateConfidence(algs),
                }
            );
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
        [HttpPost("WordEmbedding")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostWordEmbedding([FromBody] KnowledgeGraphItem item)
        {
            List<Article> articles = ar.GetArticles(item).ToList();
            IEnumerable<Passage> passages = er.GetEvidence(articles, item).Take(50);
            WordEmbedding.WordEmbedding v = new();
            foreach (var p in passages)
            {
                p.ls_score = v.GetEvidence($"{item.s} {item.r} {item.t}", p.FullPassage);
            }
            passages = passages.ToList().OrderByDescending(p => p.ls_score);
            return Ok(passages);
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

        [HttpPost("SimRank")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostSimRank([FromBody] KnowledgeGraphItem item)
        {
            return Ok($"SimRank for s({item.s},{item.t}): {sr.getSimRank(item.s, item.t)}");
        }

        [HttpPost("TF-IDF")]
        public async Task<ActionResult<KnowledgeGraphItem>> PostTfIdRewrite([FromBody] MultipleKnowledgeGraphItem item)
        {
            ArticleRetrievalHandlerV2 arv2 = new();
            List<Article> articles = arv2.GetArticles(item.Items).ToList();
            foreach (var item2 in articles)
            {
                item2.FullText = "";
                item2.TFIDF *= 10000;
            }
            return Ok(articles);
        }
    }
}