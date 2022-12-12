using FactChecker.EF;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace FactChecker.WordcountDB
{
    public class WordCount
    {
        private readonly KnoxFactCheckingTestDbContext context;
        public WordCount(KnoxFactCheckingTestDbContext context)
        {
            this.context = context;
        }
        public List<WordCountItem> FetchDB(string word)
        {
            return context.wordcount.Where(p => p.word == word).ToList();
        }

        public int FetchArticlesCountContainingWord(string word)
        {
            return context.wordcount.Where(p => p.word == word).Count();
        }

        public int FetchSumOfOccurences(string word)
        {
            return context.wordcount.Where(p => p.word == word).Sum(p => p.occurrence);
        }
        public int FetchSumOfOccurencesFromArticleId(int articleid)
        {
            return context.wordcount.Where(p => p.articleid == articleid).Sum(p => p.occurrence);
        }
        public int FetchTotalDocuments()
        {
            return context.article.Count();
        }
    }
}

