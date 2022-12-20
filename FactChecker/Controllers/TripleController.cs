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
using FactChecker.Confidence_Algorithms;
using FactChecker.Confidence_Algorithms.SimRank;

namespace FactChecker.Controllers
{

    public class AlgChooserReturn
    {
        public float Confidence { get; set; } = 0;
        public List<Article> Articles { get; set; }
    }

    [ApiController]
    [Route("Triple")]
    public class TripleController : ControllerBase
    {


        private readonly WordcountDB.stopwords stopwords;
        private readonly WordcountDB.triples triples;
        public static TestData.WikiDataTriples WikiDataTriples = new();
        readonly TMWIIS.TMWIISHandler tmwiis;
        readonly IArticleRetrieval ar;

        public TripleController(TFIDF.TFIDFHandler ar, TMWIIS.TMWIISHandler tmwiis, WordcountDB.stopwords stopwords, Cosine.CosineSim cosine, Rake.Rake rake, WordcountDB.triples triples, AdamicAdar adamic,
            Katz kats, SimRank simrank
            )
        {
            this.ar = ar;
            this.tmwiis = tmwiis;
            this.stopwords = stopwords;
            this.cosine = cosine;
            this.rake = rake;
            this.adamicAdar = adamic;
            this.katz = kats;
            this.simRank = simrank;
            this.triples = triples;
        }

        readonly IPassageRetrieval pr = new PassageRetrieval.PassageRetrievalHandler();
        readonly Cosine.CosineSim cosine;
        readonly Rake.Rake rake;
        readonly LemmatizerHandler lh = new();
        readonly WordEmbedding.WordEmbedding wordEmbedding = new();
        readonly Jaccard js = new();
        readonly Confidence_Algorithms.SimRank.SimRank simRank;
        readonly Confidence_Algorithms.AdamicAdar adamicAdar;
        readonly Confidence_Algorithms.Katz katz;

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
            => (double)cosine.similarity_v2(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
        [NonAction]
        public double CalculateWordEmbedding(AlgChooser algs, Passage passage)
            => wordEmbedding.GetEvidence(algs.MultipleKnowledgeGraphItem.ItemsAsString, passage.FullPassage);
        [NonAction]
        public float CalculateTMWIIS(AlgChooser algs, Passage passage, string FullText, int FullText_Unique)
            => algs.MultipleKnowledgeGraphItem.Items.Sum(p => tmwiis.CalculateScore(p, passage, FullText, FullText_Unique));
        [NonAction]
        public float CalculateConfidence(AlgChooser algs) => (algs.ConfidenceEnum) switch
        {
            ConfidenceEnum.SimRank => MathF.Round(simRank.GetSimRank(algs.MultipleKnowledgeGraphItem), 2),
            ConfidenceEnum.AdamicAdar => adamicAdar.CalculateScore(algs.MultipleKnowledgeGraphItem),
            ConfidenceEnum.Katz => katz.ComputeCentrality(algs.MultipleKnowledgeGraphItem),
            _ => -1f,
        };
        #endregion

        [NonAction]
        public List<Passage> PassageRanking(AlgChooser algs, List<Passage> passages, string FullText, int FullText_Unique)
        {
            Console.WriteLine($"      Ranking {passages.Count} passages");
            foreach (Passage passage in passages)
            {
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
        [Consumes("application/json")]
        public async Task<AlgChooserReturn> PostAlgChooser([FromBody] AlgChooser algs)
        {
            Console.WriteLine("Started AlgChooser");
            Console.WriteLine("Running Article Retrieval");
            List<Article> articles = ArticleRetrieval(algs);
            int i = 0;
            foreach (var art in articles)
            {
                Console.WriteLine($"   parsing article {i} of {articles.Count}");
                art.Passages = PassageExtraction(algs, art);
                art.Passages = PassageRanking(algs, art.Passages, art.FullText, art.FullText.GetUniqueWords());
                Console.WriteLine("      Done ranking passages");
                Console.WriteLine("      Generating top score for passages");
                foreach (var passage in art.Passages) passage.CalculateScoreFromKeyValuePairs();
                art.Passages = art.Passages.OrderBy(p => p.Score).ToList();
                art.FullText = art.FullText[0..100];
                Console.WriteLine("      Getting top 3 passages per article");
                art.Passages = art.Passages.Take(3).ToList();
                i++;
                Console.WriteLine($"      Done with article {i}");
            }
            Console.WriteLine("Ranked all articles and passages");
            return
                new AlgChooserReturn()
                {
                    Articles = articles.ToList(),
                    Confidence = CalculateConfidence(algs),
                };
        }
        public class HealthCheck
        {
            public string message { get; set; }
            public int status { get; set; }
            public double averageResponseTime { get; set; }
        }
        [EnableCors]
        [HttpGet("HealthCheck/v1")]
        [ProducesResponseType(200)]
        public async Task<HealthCheck> HealthCheckApi()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            await Task.Delay(1);
            watch.Stop();
            return new HealthCheck() { message="OK", status=200, averageResponseTime = watch.ElapsedMilliseconds };
        }

        [EnableCors]
        [HttpGet("Levenshtein")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<HealthCheck> LevenshteinTester(string v1, string v2)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var x = (double)LevenshteinDistanceAlgorithm.LevenshteinDistance_V2(v1, v2);
            watch.Stop();
            return new HealthCheck() { message = $"OK - {x}", status = 200, averageResponseTime = watch.ElapsedMilliseconds };
        }

        [EnableCors]
        [HttpGet("UploadStopWords")]
        [ProducesResponseType(200)]
        public async Task UploadStopWords()
        {
            await stopwords.UploadAllStopWords();
        }

        [EnableCors]
        [HttpGet("UploadTriples")]
        [ProducesResponseType(200)]
        public async Task UploadTriples()
        {
            triples.UploadAllRelations();
        }
    }
}