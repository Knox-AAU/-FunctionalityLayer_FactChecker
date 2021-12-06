using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class WordCountItem
    {
        public int WordID { get; set; }
        public string Word { get; set; }
        public int ArticleID { get; set; }
        public int Occurrence { get; set; }

        public WordCountItem(int id, string word, int articleid, int occurrence)
        {
            WordID = id;
            Word = word;
            ArticleID = articleid;
            Occurrence = occurrence;
        }
        public override string ToString()
        {
            return "ID:" + WordID + " Word:" + Word + " ArticleID:" + ArticleID + " Occurrence:" + Occurrence;
        }
    }
}
