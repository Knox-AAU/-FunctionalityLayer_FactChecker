using System.Collections.Generic;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class MultipleKnowledgeGraphItem
    {
        public List<KnowledgeGraphItem> items { get; set; }
        public string passage { get; set; }
    }
}
