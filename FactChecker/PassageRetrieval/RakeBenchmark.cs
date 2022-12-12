using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.Interfaces;
using FactChecker.PassageRetrieval;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactChecker.Rake
{
    [InProcess]
    [MemoryDiagnoser]
    public class RakeBenchmark
    {
        private static string __benchmark_article_text = "";


        [Params(50, 75, 100)]
        public int __passage_length { get; set; }
        [Benchmark]
        public void Rake_Retrieval_with_stopwords()
        {
            Rake rake = new Rake(sentences_min_length: __passage_length);
            Article article = new() { FullText = __benchmark_article_text };
            List<Passage> s = rake.GetPassages(article).ToList();
        }
        [Benchmark]
        public void Rake_Retrieval_without_stopwords()
        {
            Rake rake = new Rake(stopwords: new() { } ,sentences_min_length: __passage_length);
            Article article = new() { FullText = __benchmark_article_text };
            List<Passage> s = rake.GetPassages(article).ToList();
        }
        [Benchmark]
        public void Rake_Retrieval_Get_FullPassages()
        {
            Rake rake = new Rake(sentences_min_length: __passage_length, get_Only_Sentences: true);
            Article article = new() { FullText = __benchmark_article_text };
            List<Passage> s = rake.GetPassages(article).ToList();
        }

        public static void RunBenchmarks()
        {
            __benchmark_article_text = File.ReadAllText("./PassageRetrieval/Benchmark_article_text.txt");
            var summary = BenchmarkRunner.Run<RakeBenchmark>();
        }
    }
}