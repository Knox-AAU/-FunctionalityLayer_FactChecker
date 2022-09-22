using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.Intefaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactChecker.PassageRetrieval
{
    [InProcess]
    [MemoryDiagnoser]
    public class PassageRetrievalBenchmarks
    {
        private static string __benchmark_article_text = "";
        private static int __benchmark_passage_length = 10;
        private readonly static int __benchmark_passage_overlap = 5;
        [Benchmark]
        public void Old_Passage_Retrieval()
        {
            PassageRetrievalHandler pi = new()
            {
                PassageLength = __benchmark_passage_length,
                PassageOverlap = __benchmark_passage_overlap
            };
            Article article = new() { FullText = __benchmark_article_text };
            List<Passage> s = pi.GetPassages(article).ToList();
        }
        [Benchmark]
        public void New_Passage_Retrieval()
        {
            PassageRetrievalHandler pi = new()
            {
                PassageLength = __benchmark_passage_length,
                PassageOverlap = __benchmark_passage_overlap
            };
            Article article = new() { FullText = __benchmark_article_text };
            List<string> s = pi.GetPassage_new(article.FullText).ToList();
        }
        [Benchmark]
        public void New_Passage_Retrieval_V2()
        {
            PassageRetrievalHandler pi = new()
            {
                PassageLength = __benchmark_passage_length,
                PassageOverlap = __benchmark_passage_overlap
            };
            Article article = new() { FullText = __benchmark_article_text };
            List<string> s = pi.GetPassage_new_V2(article.FullText).ToList();
        }
        [Benchmark]
        public void New_Passage_Retrieval_V3()
        {
            PassageRetrievalHandler pi = new()
            {
                PassageLength = __benchmark_passage_length,
                PassageOverlap = __benchmark_passage_overlap
            };
            Article article = new() { FullText = __benchmark_article_text };
            List<string> s = pi.GetPassage_new_V3(article.FullText).ToList();
        }
        public static void RunBenchmarks()
        {
            __benchmark_article_text = File.ReadAllText("./PassageRetrieval/Benchmark_article_text.txt");
            var summary = BenchmarkRunner.Run<PassageRetrievalBenchmarks>();
        }
    }
}
