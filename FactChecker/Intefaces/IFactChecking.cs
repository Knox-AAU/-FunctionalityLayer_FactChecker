using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using static FactChecker.Controllers.AlgChooser;

namespace FactChecker.Interfaces
{
    public interface IPassageRetrieval
    {
        public IEnumerable<Passage> GetPassages(Article _);
    }
    public interface IArticleRetrieval
    {
        public IEnumerable<Article> GetArticles(List<KnowledgeGraphItem> items);
        public IEnumerable<Article> GetArticles(KnowledgeGraphItem item);
    }

    public interface IEvidenceRetrieval
    {
        public IEnumerable<Passage> GetEvidence(List<Article> articles, List<KnowledgeGraphItem> items);
        public IEnumerable<Passage> GetEvidence(List<Article> articles, KnowledgeGraphItem item);
    }

    public class Passage
    {
        public string FullPassage { get; set; }
        public double Score { get; set; }
        public int ls_rank { get; set; } = 0;
        public double ls_score { get; set; } = 0;
        public int js_rank { get; set; } = 0;
        public double js_score { get; set; } = 0;
        public float rake_rank { get; set; } = 0;
        public double JaccardScore { get; set; } = 0;
        public int Artical_ID { get; set; }
        public Dictionary<PassageRankingEnum, double> KeyValuePairs { get; set; } = new();
        public List<string> ProcessedPassage { get; set; }
        public double LevenshteinScore { get; set; } = 0;
        public double CosineScore { get; set; } = 0;
        public double WordEmbeddingScore { get; set; } = 0;
        public string ProcessedPassageAsString { get {
                if (ProcessedPassage != null)
                {
                    return string.Join(' ', ProcessedPassage);
                }
                else { 
                    return string.Empty;
                }
            } }


        public Passage()
        {
        }
        public Passage(string fullPassage, List<string> prosecsPassage)
        {
            FullPassage = fullPassage;
            ProcessedPassage = prosecsPassage;
        }

        internal double GetPassageRankingWeight(PassageRankingEnum PRE)
        {
            return 1;
        }

        internal void CalculateScoreFromKeyValuePairs()
        {
            foreach (KeyValuePair<PassageRankingEnum, double> entry in KeyValuePairs)
                this.Score += entry.Value * GetPassageRankingWeight((PassageRankingEnum)entry.Key);
        }
    }
    public static class PassageListExtensions
    {
        public static void RankJaccard(this List<Passage> passages)
        {
            passages = passages.OrderByDescending(p => p.JaccardScore).ToList();
            for (int i = 0; i < passages.Count; i++)
            {
                var passage = passages[i];
                passage.KeyValuePairs.Remove(PassageRankingEnum.Jaccard);
                passage.KeyValuePairs.Add(PassageRankingEnum.Jaccard, i + 1);
            }
        }
        public static void RankWordEmbedding(this List<Passage> passages)
        {
            passages = passages.OrderByDescending(p => p.WordEmbeddingScore).ToList();
            for (int i = 0; i < passages.Count; i++)
            {
                var passage = passages[i];
                passage.KeyValuePairs.Remove(PassageRankingEnum.WordEmbedding);
                passage.KeyValuePairs.Add(PassageRankingEnum.WordEmbedding, i + 1);
            }
        }
        public static void RankWordLevenshtein(this List<Passage> passages)
        {
            passages = passages.OrderBy(p => p.LevenshteinScore).ToList();
            for (int i = 0; i < passages.Count; i++)
            {
                var passage = passages[i];
                passage.KeyValuePairs.Remove(PassageRankingEnum.Levenshtein);
                passage.KeyValuePairs.Add(PassageRankingEnum.Levenshtein, i + 1);
            }
        }
        public static void RankCosine(this List<Passage> passages)
        {
            passages = passages.OrderByDescending(p => p.CosineScore).ToList();
            for (int i = 0; i < passages.Count; i++)
            {
                var passage = passages[i];
                passage.KeyValuePairs.Remove(PassageRankingEnum.Cosine);
                passage.KeyValuePairs.Add(PassageRankingEnum.Cosine, i + 1);
            }
        }
}
    public class Article
    {
        public int Id { get; set; }
        public string FullText { get; set; }
        public List<WordcountDB.WordCountItem> WordCountItems { get; set; }
        public double? TFIDF { get; set; }
        public List<Passage> Passages { get; set; } = new();
    }
}
