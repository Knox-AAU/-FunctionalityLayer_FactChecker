using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TFIDF
{
    public class TFIDFItem
    {
        public int articleId;
        public float score;

        /// <summary>
        /// Constructor taking two arguments of types (<typeparamref name="int"/>, <typeparamref name="float"/>)
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="score">TF-IDF score</param>
        public TFIDFItem(int articleId, float score)
        {
            this.articleId = articleId;
            this.score = score;
        }
    }
}
