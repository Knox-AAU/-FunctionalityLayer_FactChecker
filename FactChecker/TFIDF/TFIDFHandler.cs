using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TFIDF
{
    /// <summary>
    /// Contains the method <c>CalculateTFIDF</c>
    /// </summary>
    public class TFIDFHandler
    {
        int numberOfArticles = 15;
        public int maxArticles = 5;

        /// <summary>
        /// Takes a list of <typeparamref name="string"/> 
        /// and ranks articles using each <typeparamref name="string"/> in the list.
        /// </summary>
        /// <param name="search">List containing words from the relation triple</param>
        /// <returns>
        /// A list containing the Top 5 articles which are most likely 
        /// to support the <typeparamref name="search"/> parameter.
        /// </returns>
        public List<TFIDFItem> CalculateTFIDF (List<string> search)
        {
            WordcountDB.WordCount wordCount = new WordcountDB.WordCount();
            List<TFIDFItem> articles = new List<TFIDFItem>();
            foreach(string s in search) //In this iteration of the project, search is a list of words from the chosen triple 
            {                           
                List<WordcountDB.WordCountItem> wordcountItems = wordCount.FetchDB(s);
                foreach(WordcountDB.WordCountItem item in wordcountItems)
                {
                    float tf = CalculateTermFrequency(item.Occurrence);
                    float idf = CalculateInverseDocumentFrequency(numberOfArticles, wordcountItems.Count);
                    bool foundArticle = false;
                    foreach(TFIDFItem article in articles) //Add TF-IDF score to specific article which contain s
                    {
                        if(article.articleId == item.ArticleID)
                        {
                            article.score += tf * idf;
                            foundArticle = true;
                        }
                    }
                    if(!foundArticle) //If an article containing s is not found, a new TFIDFItem is added to a list
                    {
                        TFIDFItem article = new TFIDFItem(item.ArticleID, tf * idf);
                        articles.Add(article);
                    }
                }
            }
            articles.Sort((p, q) => q.score.CompareTo(p.score));
            return articles.Take(maxArticles).ToList();
        }

        /// <summary>
        /// Takes the amount of times a specific word occurs in an article and calculates termfrequecy.
        /// </summary>
        /// <param name="f_td">The amount of times a specific word occurs in an article</param>
        /// <returns>
        /// Term frequenzy of type <typeparamref name="float"/>
        /// </returns>
        public float CalculateTermFrequency (int f_td)
        {
            return (float)(1 + Math.Log(f_td,10));
        }

        /// <summary>
        /// Takes the total number of documents: type <typeparamref name="numberOfDocuments"/>,
        /// and a unique number of documents containing a particular term: type <typeparamref name="int"/>.
        /// </summary>
        /// <param name="numberOfDocuments">Total number of documents</param>
        /// <param name="numberOfDocumentsWithTerm">Number of unique documents containing a partcular term</param>
        /// <returns>
        /// Inverse document frequenzy of type <typeparamref name="float"/>
        /// </returns>
        public float CalculateInverseDocumentFrequency (int numberOfDocuments, int numberOfDocumentsWithTerm)
        {
            return (float)Math.Log(numberOfDocuments / numberOfDocumentsWithTerm,10);
        }
    }
}
