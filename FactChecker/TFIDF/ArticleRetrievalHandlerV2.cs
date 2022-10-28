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
        readonly WordcountDB.Article articleHandler;
        readonly WordCount wordCount;

        public ArticleRetrievalHandlerV2(WordcountDB.Article articleHandler, WordCount wordCount) : this()
        {
            this.articleHandler = articleHandler;
            this.wordCount = wordCount;
        }

        List<Interfaces.Article> Articles = new();
        
        List<string> searchItems = new();
        private readonly double __total_documents;


        protected ArticleRetrievalHandlerV2()
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
            List<int> ArticleIds = wcItems.Select(p => p.articleid).GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            foreach (var item in ArticleIds)
            {
                CalculateTfIdf(wcItems.Where(p => p.articleid == item).ToList());
            }
            return Articles.OrderByDescending(p => p.TFIDF).ToList();
        }

        public void CalculateTfIdf(List<WordCountItem> items)
        {
            string fulltext = articleHandler.FetchDB(items.First().articleid).text;
            int fulltext_count = wordCount.FetchSumOfOccurencesFromArticleId(items.First().articleid);
            Interfaces.Article article = new Interfaces.Article()
            {
                FullText = fulltext,
                Id = items.First().articleid,
                WordCountItems = new List<WordCountItem>()
            };
            article.TFIDF = 0;
            foreach (var item in items)
            {
                item.TF = item.occurrence / (double)fulltext_count;
                item.IDF = Math.Log10(__total_documents / (1 + wordCount.FetchArticlesCountContainingWord(item.word)));
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
