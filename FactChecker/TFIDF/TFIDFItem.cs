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

        public TFIDFItem(int articleId, float score)
        {
            this.articleId = articleId;
            this.score = score;
        }
    }
}
