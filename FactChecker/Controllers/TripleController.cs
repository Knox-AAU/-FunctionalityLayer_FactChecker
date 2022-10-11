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
    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public static TestData.WikiDataTriples WikiDataTriples = new();
        readonly IEvidenceRetrieval er = new TMWIIS.TMWIISHandler();
        readonly IArticleRetrieval ar = new TFIDF.TFIDFHandler();
        readonly IPassageRetrieval pr = new PassageRetrieval.PassageRetrievalHandler();
        readonly IPassageRetrieval rake = new Rake.Rake();
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

        [HttpPost("AlgChooser")]
        public async Task<ActionResult<List<Article>>> PostAlgChooser([FromBody] AlgChooser algs)
        {
            List<Article> articles;
            articles = algs.ArticleRetrieval switch
            {
                ArticleRetrievalEnum.TF_IDF => ar.GetArticles(algs.MultipleKnowledgeGraphItem.Items).ToList(),
                _ => ar.GetArticles(algs.MultipleKnowledgeGraphItem.Items).ToList()
            };
            foreach (var art in articles)
            {
                art.Passages = algs.PassageExtraction switch
                {
                    PassageExtractionEnum.Default => pr.GetPassages(art).Take(50).ToList(),
                    PassageExtractionEnum.Rake => rake.GetPassages(art).Take(50).ToList(),
                    _ => pr.GetPassages(art).ToList()
                };
                foreach (Passage passage in art.Passages)
                {
                    foreach (var passageRanking in algs.PassageRankings)
                    {
                        switch (passageRanking)
                        {
                            case PassageRankingEnum.Jaccard:
                                double jaccard_value = (double)Math.Round(js.Similarity(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage), 2);
                                passage.JaccardScore = jaccard_value;
                                break;
                            case PassageRankingEnum.Levenshtein:
                                double leven_value = (double)LevenshteinDistanceAlgorithm.LevenshteinDistance_V2(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
                                passage.LevenshteinScore = leven_value;
                                break;
                            case PassageRankingEnum.Cosine:
                                double cosine_value = (double)new Cosine.CosineSim().similarity_v2(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
                                passage.CosineScore = cosine_value;
                                break;
                            case PassageRankingEnum.WordEmbedding:
                                double wordEmbedding_value = wordEmbedding.GetEvidence(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
                                passage.WordEmbeddingScore = wordEmbedding_value;
                                break;
                        }
                    }
                }

                foreach (var passageRanking in algs.PassageRankings)
                {
                    switch (passageRanking)
                    {

                        case PassageRankingEnum.Jaccard:
                            art.Passages = art.Passages.OrderByDescending(p => p.JaccardScore).ToList();
                            for (int i = 0; i < art.Passages.Count; i++)
                            {
                                var passage = art.Passages[i];
                                passage.KeyValuePairs.Remove(passageRanking);
                                passage.KeyValuePairs.Add(passageRanking, i + 1);
                            }
                            break;
                        case PassageRankingEnum.Cosine:
                            art.Passages = art.Passages.OrderByDescending(p => p.CosineScore).ToList();
                            for (int i = 0; i < art.Passages.Count; i++)
                            {
                                var passage = art.Passages[i];
                                passage.KeyValuePairs.Remove(passageRanking);
                                passage.KeyValuePairs.Add(passageRanking, i + 1);
                            }
                            break;
                        case PassageRankingEnum.WordEmbedding:
                            art.Passages = art.Passages.OrderByDescending(p => p.WordEmbeddingScore).ToList();
                            for (int i = 0; i < art.Passages.Count; i++)
                            {
                                var passage = art.Passages[i];
                                passage.KeyValuePairs.Remove(passageRanking);
                                passage.KeyValuePairs.Add(passageRanking, i + 1);
                            }
                            break;
                        case PassageRankingEnum.Levenshtein:
                            art.Passages = art.Passages.OrderBy(p => p.LevenshteinScore).ToList();
                            for (int i = 0; i < art.Passages.Count; i++)
                            {
                                var passage = art.Passages[i];
                                passage.KeyValuePairs.Remove(passageRanking);
                                passage.KeyValuePairs.Add(passageRanking, i + 1);
                            }
                            break;
                    }
                }
                foreach (var passage in art.Passages) passage.CalculateScoreFromKeyValuePairs();
                art.Passages = art.Passages.OrderBy(p => p.Score).ToList();
                art.FullText = art.FullText[0..100];
                art.Passages = art.Passages.Take(3).ToList();
            }
            return Ok(articles);
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