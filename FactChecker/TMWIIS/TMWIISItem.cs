using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TMWIIS
{
    public class TMWIISItem
    {
        public double score;
        public string passage;
        public string link;

        public TMWIISItem(double Score, string Passage, string Link)
        {
            score = Score;
            passage = Passage;
            link = Link;
        }
    }
}
