using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class KnowledgeGraphItem
    {
        public string s { get; set; }
        public string r { get; set; }
        public string t { get; set; }
        public string passage { get; set; }

        /// <summary>
        /// Contructor taking three arguments of type (<paramref name="string"/>, <paramref name="string"/>, <paramref name="string"/>).
        /// Used to create relation triples from the knowledge graph. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <param name="t"></param>
        public KnowledgeGraphItem (string s, string r, string t)
        {
            this.s = s;
            this.r = r;
            this.t = t;
        }

        /// <summary>
        /// Constructor taking no arguments. Used for working with knowledge graph data that does not come in triples. 
        /// </summary>
        public KnowledgeGraphItem()
        {

        }

        public override string ToString()
        {
            return "<" + s + ">" + " <"+r + "> <" + t+">";
        }
    }
}
