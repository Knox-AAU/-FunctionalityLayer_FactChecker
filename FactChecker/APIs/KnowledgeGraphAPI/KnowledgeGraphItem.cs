using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class KnowledgeGraphItem
    {
        public string s;
        public string r;
        public string t;
        public KnowledgeGraphItem (string s, string r, string t)
        {
            this.s = s;
            this.r = r;
            this.t = t;
        }

        public override string ToString()
        {
            return s + " " + r + " " + t;
        }
    }
}
