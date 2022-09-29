using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Intefaces;
using FactChecker.WordcountDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.PassageRetrieval
{
    public class ArticleRetrievalHandlerV2 : IArticleRetrieval
    {
        List<Intefaces.Article> Articles = new();
        WordcountDB.Article articleHandler = new();
        WordCount wordCount = new();
        List<string> searchItems = new();

        public IEnumerable<Intefaces.Article> GetArticles(List<KnowledgeGraphItem> items)
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
            Intefaces.Article article = new Intefaces.Article()
            {
                FullText = fulltext,
                Id = items.First().ArticleID,
                WordCountItems = new List<WordCountItem>()
            };
            article.TFIDF = 0;
            foreach (var item in items)
            {
                item.TF = (double)item.Occurrence / (double)fulltext_count;
                item.IDF = Math.Log10(1 + (double)wordCount.FetchArticlesCountContainingWord(item.Word) / (double)wordCount.FetchTotalDocuments());
                article.TFIDF += item.TF * item.IDF;
            }
            article.WordCountItems.AddRange(items);
            Articles.Add(article);
        }

        public IEnumerable<Intefaces.Article> GetArticles(KnowledgeGraphItem item)
        {
            return GetArticles(new List<KnowledgeGraphItem>() { item });
        }
    }
}
