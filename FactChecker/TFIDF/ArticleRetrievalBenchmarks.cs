using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
using FactChecker.PassageRetrieval;
using FactChecker.WordcountDB;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactChecker.TFIDF
{
    [InProcess]
    [MemoryDiagnoser]
    public class ArticleRetrievalBenchmarks
    {
        public static List<ArticleItem> Articles { get; set; }
        public List<KnowledgeGraphItem> Results { get; set; } = new () { new("Holland", "", "") };

        [Benchmark]
        public void Old_TfIDF()
        {
            TFIDFHandler tf = new();
            tf.GetArticles(Results);
        }
        [Benchmark]
        public void New_TfIDF()
        {
            ArticleRetrievalHandlerV2 ar = new();
            ar.GetArticles(Results);
        }


        public static void RunBenchmarks()
        {
            WordcountDB.Article a = new();
            Articles = new();
            for (int i = 0; i < 10; i++)
                Articles.Add(a.FetchDB(i));
            var summary = BenchmarkRunner.Run<ArticleRetrievalBenchmarks>();
        }
    }
}
