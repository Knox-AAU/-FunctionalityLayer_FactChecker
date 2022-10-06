using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Data.SQLite;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.WordcountDB;

namespace XUnitTestProject
{
    public class TFIDFTesting
    {
        [Fact]
        public void TestCalculateTfIdf()
        {
            int fulltext_count = 5;
            int TotalDocs = 2;
            FactChecker.Interfaces.Article article = new()
            {
                FullText = "",
                Id = -1,
                WordCountItems = new List<WordCountItem>()
            };
            var items = new List<WordCountItem>() { 
                new WordCountItem(0, "this", 0, 1),
                new WordCountItem(0, "is", 0, 1),
                new WordCountItem(0, "a", 0, 2),
                new WordCountItem(0, "sample", 0, 1)
            };
            List<int> articleCountContainingWords = new List<int>() { 2, 2, 1, 1 };
            article.TFIDF = 0;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.TF = (double)item.Occurrence / (double)fulltext_count;
                item.IDF = Math.Log10(1 + (double)articleCountContainingWords[i] / TotalDocs);
                article.TFIDF = item.TF * item.IDF;
                switch (i)
                {
                    case 0:
                        Assert.Equal(0.2, item.TF, 0);
                        Assert.Equal(0.301, item.IDF, 3);
                        break;
                    case 1:
                        Assert.Equal(0.2, item.TF, 0);
                        Assert.Equal(0.301, item.IDF, 3);
                        break;
                    case 2:
                        Assert.Equal(0.4, item.TF, 0);
                        Assert.Equal(0.176, item.IDF, 3);
                        break;
                    case 4:
                        Assert.Equal(0.2, item.TF, 0);
                        Assert.Equal(0.176, item.IDF, 3);
                        break;
                }

            }
        }
    }
}
