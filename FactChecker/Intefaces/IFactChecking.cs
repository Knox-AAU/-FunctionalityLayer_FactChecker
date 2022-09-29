using FactChecker.APIs.KnowledgeGraphAPI;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.Intefaces
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
        public string Text { get; set; }
        public double Score { get; set; }
        public int ls_rank { get; set; } = 0;
        public double ls_score { get; set; } = 0;
        public int js_rank { get; set; } = 0;
        public double js_score { get; set; } = 0;
    }
    public class Article
    {
        public int Id { get; set; }
        public string FullText { get; set; }
        public List<WordcountDB.WordCountItem> WordCountItems { get; set; }
        public double? TFIDF { get; set; }
    }
}
