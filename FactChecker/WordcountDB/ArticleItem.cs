using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class ArticleItem
    {
        public int id { get; set; }
        public string link { get; set; }
        public int length { get; set; }
        public int unique_length { get; set; }
        public string text { get; set; }


        /// <summary>
        /// Constructor taking five arguments of types 
        /// (<typeparamref name="int"/>, <typeparamref name="string"/>, <typeparamref name="int"/>, <typeparamref name="int"/>, <typeparamref name="string"/>).
        /// Used to create new articles when fetching from the database using ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="link"></param>
        /// <param name="lenght"></param>
        /// <param name="unique_lenght"></param>
        /// <param name="text"></param>
        public ArticleItem()
        {

        }
        public override string ToString() 
        {
            return "ID:" + id + " Link:" + link + " Lenght:" + length + " Unique Lenght:" + unique_length + " Text:" + text;
        }
    }
}
