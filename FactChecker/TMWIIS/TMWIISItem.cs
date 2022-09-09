using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TMWIIS
{
    public class TMWIISItem
    {
        public float score { get; set; }
        public string passage { get; set; }
        public string link { get; set; }

        public TMWIISItem(float Score, string Passage, string Link)
        {
            score = Score;
            passage = Passage;
            link = Link;
        }
    }
}
