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
using Microsoft.AspNetCore.Cors;

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
        readonly TMWIIS.TMWIISHandler tmwiis = new TMWIIS.TMWIISHandler();
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
        public float CalculateTMWIIS(AlgChooser algs, Passage passage, string FullText, int FullText_Unique)
            => algs.MultipleKnowledgeGraphItem.Items.Sum(p => tmwiis.CalculateScore(p, passage, FullText, FullText_Unique));
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
        public List<Passage> PassageRanking(AlgChooser algs, List<Passage> passages, string FullText, int FullText_Unique)
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
                        case PassageRankingEnum.TMWIIS:
                            passage.TMWIISScore = CalculateTMWIIS(algs, passage, FullText, FullText_Unique);
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
                    case PassageRankingEnum.TMWIIS:
                        passages.RankTMWIIS();
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
        [EnableCors]
        [HttpPost("AlgChooser")]
        [ProducesResponseType(typeof(AlgChooserReturn), 200)]
        [Produces("application/json")]
        public async Task<ActionResult<List<Article>>> PostAlgChooser([FromBody] AlgChooser algs)
        {
            List<Article> articles = ArticleRetrieval(algs);
            foreach (var art in articles)
            {
                art.Passages = PassageExtraction(algs, art);
                art.Passages = PassageRanking(algs, art.Passages, art.FullText, art.FullText.GetUniqueWords());
                foreach (var passage in art.Passages) passage.CalculateScoreFromKeyValuePairs();
                art.Passages = art.Passages.OrderBy(p => p.Score).ToList();
                art.FullText = art.FullText[0..100];
                art.Passages = art.Passages.Take(50).ToList();
            }
            return Ok(
                new AlgChooserReturn()
                {
                    Articles = articles.Take(1).ToList(),
                    Confidence = CalculateConfidence(algs),
                }
            );
        }

        
    }
}