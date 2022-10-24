using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.Interfaces;
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
        public int __benchmark_passage_length { get; set; }
        [Benchmark]
        public void Rake_Passage_Retrieval()
        {
            Rake rake = new Rake(sentences_min_length: __benchmark_passage_length);
            Article article = new() { FullText = __benchmark_article_text };
            List<Passage> s = rake.GetPassages(article).ToList();
        }
    }
}