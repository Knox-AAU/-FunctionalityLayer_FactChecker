using FactChecker.APIs.KnowledgeGraphAPI;
using System.Collections.Generic;

namespace FactChecker.Controllers
{
    public class AlgChooser
    {

        public PassageExtractionEnum PassageExtraction { get; set; } = PassageExtractionEnum.Default;
        public ArticleRetrievalEnum ArticleRetrieval { get; set; } = ArticleRetrievalEnum.TF_IDF;
        public ConfidenceEnum ConfidenceEnum { get; set; } = ConfidenceEnum.Disabled;
        public List<PassageRankingEnum> PassageRankings { get; set; } = new() { PassageRankingEnum.Levenshtein, PassageRankingEnum.Cosine };
        public MultipleKnowledgeGraphItem MultipleKnowledgeGraphItem { get; set; }
    }
    public enum ConfidenceEnum
    {
        Disabled,
        SimRank
    }
    public enum PassageExtractionEnum
    {
        Default,
        Rake
    }
    public enum ArticleRetrievalEnum
    {
        TF_IDF
    }
    public enum PassageRankingEnum
    {
        Levenshtein,
        Jaccard,
        Cosine,
        WordEmbedding
    }
}