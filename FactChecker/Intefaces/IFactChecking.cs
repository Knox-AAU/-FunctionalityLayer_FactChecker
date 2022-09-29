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
        public static int ID = 0;
        public string FullPassage { get; set; }
        public double Score { get; set; }
        public int ls_rank { get; set; } = 0;
        public double ls_score { get; set; } = 0;
        public int js_rank { get; set; } = 0;
        public double js_score { get; set; } = 0;
        public float rake_rank { get; set; } = 0;
        public int Artical_ID { get; set; }
        public List<string> ProsecsPassage { get; set; }
        public string ProsecsPassageAsString { get {
                if (ProsecsPassage != null)
                {
                    return string.Join(' ', ProsecsPassage);
                }
                else { 
                    return string.Empty;
                }
            } }
        public Passage()
        {
            ID = ID + 1;
        }
        public Passage(string fullPassage, List<string> prosecsPassage)
        {
            ID = ID + 1;
            FullPassage = fullPassage;
            ProsecsPassage = prosecsPassage;
        }

    }
    public class Article
    {
        public int Id { get; set; }
        public string FullText { get; set; }
    }
}
