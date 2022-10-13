using System.Collections.Generic;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class MultipleKnowledgeGraphItem
    {
        public List<KnowledgeGraphItem> Items { get; set; }
        public string passage { get; set; }

        public string ItemsAsString { get
            {
                if (Items == null || Items.Count == 0) return string.Empty;
                string x = "";
                foreach (var item in Items)
                    x += $"{item.s} {item.r} {item.t}";
                return x;    
            } }
    }
}
