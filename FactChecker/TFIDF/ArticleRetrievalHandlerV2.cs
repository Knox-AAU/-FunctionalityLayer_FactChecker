using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
using FactChecker.WordcountDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.TFIDF
{
    public class ArticleRetrievalHandlerV2 : IArticleRetrieval
    {
        List<Interfaces.Article> Articles = new();
        WordcountDB.Article articleHandler = new();
        WordCount wordCount = new();
        List<string> searchItems = new();
        private readonly double __total_documents;


        public ArticleRetrievalHandlerV2()
        {
            __total_documents = wordCount.FetchTotalDocuments();
        }
        public IEnumerable<Interfaces.Article> GetArticles(List<KnowledgeGraphItem> items)
        {
            searchItems = items.Select(p => new List<string>() { p.s, p.r, p.t }).SelectMany(l => l).Distinct().ToList();
            List<WordCountItem> wcItems = new();
            searchItems.ForEach(p =>
            {
                wcItems.AddRange(wordCount.FetchDB(p));
            });
            List<int> ArticleIds = wcItems.Select(p => p.ArticleID).GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            foreach (var item in ArticleIds)
            {
                CalculateTfIdf(wcItems.Where(p => p.ArticleID == item).ToList());
            }
            return Articles.OrderByDescending(p => p.TFIDF).ToList();
        }

        public void CalculateTfIdf(List<WordCountItem> items)
        {
            string fulltext = articleHandler.FetchDB(items.First().ArticleID).Text;
            int fulltext_count = wordCount.FetchSumOfOccurencesFromArticleId(items.First().ArticleID);
            Interfaces.Article article = new Interfaces.Article()
            {
                FullText = fulltext,
                Id = items.First().ArticleID,
                WordCountItems = new List<WordCountItem>()
            };
            article.TFIDF = 0;
            foreach (var item in items)
            {
                item.TF = item.Occurrence / (double)fulltext_count;
                item.IDF = Math.Log10(1 + wordCount.FetchArticlesCountContainingWord(item.Word) / __total_documents);
                article.TFIDF += item.TF * item.IDF;
            }
            article.WordCountItems.AddRange(items);
            Articles.Add(article);
        }

        public IEnumerable<Interfaces.Article> GetArticles(KnowledgeGraphItem item)
        {
            return GetArticles(new List<KnowledgeGraphItem>() { item });
        }
    }
}
