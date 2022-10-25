using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class WordCountItem
    {
        public int id { get; set; }
        public string word { get; set; }
        public int articleid { get; set; }
        public int occurrence { get; set; }

        public double TF;
        public double IDF;
        public WordCountItem(int id, string word, int articleid, int occurrence)
        {
            this.id = id;
            this.word = word;
            this.articleid = articleid;
            this.occurrence = occurrence;
        }
        public WordCountItem()
        {

        }
        public override string ToString()
        {
            return "ID:" + id + " Word:" + word + " ArticleID:" + articleid + " Occurrence:" + occurrence;
        }
    }
}
