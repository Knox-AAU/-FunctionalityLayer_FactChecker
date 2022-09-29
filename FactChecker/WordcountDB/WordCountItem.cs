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

        public double TF;
        public double IDF;

        /// <summary>
        /// Constructor taking four arguments of type 
        /// (<typeparamref name="int"/>, <typeparamref name="string"/>, <typeparamref name="int"/>, <typeparamref name="int"/>).
        /// Used to create new articles when fetching from the database using a specific word.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="word"></param>
        /// <param name="articleid"></param>
        /// <param name="occurrence"></param>
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
