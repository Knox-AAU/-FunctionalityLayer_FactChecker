using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TFIDF
{
    public class TFIDFHandler
    {
        
        int numberOfArticles = 16;
        public int maxArticles = 4;
        public List<TFIDFItem> CalculateTFIDF (List<string> search)
        {
            WordcountDB.WordCount wordCount = new WordcountDB.WordCount();
            List<TFIDFItem> articles = new List<TFIDFItem>();
            foreach(string s in search)
            {
                List<WordcountDB.WordCountItem> wordcountItems = wordCount.FetchDB(s);
                foreach(WordcountDB.WordCountItem item in wordcountItems)
                {
                    float tf = CalculateTermFrequency(item.Occurrence);
                    float idf = CalculateInverseDocumentFrequency(numberOfArticles, wordcountItems.Count);
                    bool foundArticle = false;
                    foreach(TFIDFItem article in articles)
                    {
                        if(article.articleId == item.ArticleID)
                        {
                            article.score += tf * idf;
                            foundArticle = true;
                        }
                    }
                    if(!foundArticle)
                    {
                        TFIDFItem article = new TFIDFItem(item.ArticleID, tf * idf);
                        articles.Add(article);
                    }
                }
            }
            articles.Sort((p, q) => q.score.CompareTo(p.score));
            return articles.Take(maxArticles).ToList();
        }

        public float CalculateTermFrequency (int f_td)
        {
            return (float)(1 + Math.Log(f_td,10));
        }

        public float CalculateInverseDocumentFrequency (int numberOfDocuments, int numberOfDocumentsWithTerm)
        {
            return (float)Math.Log(numberOfDocuments / numberOfDocumentsWithTerm,10);
        }
    }
}
