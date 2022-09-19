using FactChecker.APIs.KnowledgeGraphAPI;
using System.Collections.Generic;

namespace FactChecker.Intefaces
{
    public interface IPassageRetrieval
    {
        public List<Passage> GetPassages(List<Article> articles, List<KnowledgeGraphItem> items);
        public List<Passage> GetPassages(List<Article> articles, KnowledgeGraphItem item);
    }
    public interface IArticleRetrieval
    {
        public List<Article> GetArticles(List<KnowledgeGraphItem> items);
        public List<Article> GetArticles(KnowledgeGraphItem item);
    }

    public interface IEvidenceRetrieval : IPassageRetrieval, IArticleRetrieval
    {
        public List<Passage> GetEvidence(List<KnowledgeGraphItem> items)
        {
            return GetPassages(GetArticles(items), items);
        }
        public List<Passage> GetEvidence(KnowledgeGraphItem item)
        {
            return GetPassages(GetArticles(item), item);
        }
    }

    public class Passage
    {
        public string text { get; set; }
    }
    public class Article
    {
        public int Id { get; set; }
    }
}
