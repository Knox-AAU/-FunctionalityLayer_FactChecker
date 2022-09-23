using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.APIs.KnowledgeGraphAPI;

namespace FactChecker.BenchMarks
{
    [InProcess]
    [MemoryDiagnoser]
    public class PostBenchMark
    {
        //[Benchmark]
        //public KnowledgeGraphItem PostPassageFound()
        //{
        //    KnowledgeGraphItem item = new KnowledgeGraphItem("Zendaya", "", "");
        //    TFIDF.TFIDFHandler tFIDFHandler = new();
        //    List<string> searchList = new() { item.s, item.r, item.t };
        //    List<TFIDF.TFIDFItem> tFIDFItems = tFIDFHandler.CalculateTFIDF(searchList);
        //    List<int> articles = tFIDFItems.Select(p => p.articleId).ToList();
        //    TMWIIS.TMWIISHandler tMWIISHandler = new(articles, item);
        //    item.passage = tMWIISHandler.Evidence().OrderByDescending(p => p.score).FirstOrDefault()?.passage ?? "No Passage found";
        //    return item;
        //}
        //[Benchmark]
        //public KnowledgeGraphItem PostNoPassageFound()
        //{
        //    KnowledgeGraphItem item = new KnowledgeGraphItem("Zendaya", "", "");
        //    TFIDF.TFIDFHandler tFIDFHandler = new();
        //    List<string> searchList = new() { item.s, item.r, item.t };
        //    List<TFIDF.TFIDFItem> tFIDFItems = tFIDFHandler.CalculateTFIDF(searchList);
        //    List<int> articles = tFIDFItems.Select(p => p.articleId).ToList();
        //    TMWIIS.TMWIISHandler tMWIISHandler = new(articles, item);
        //    item.passage = tMWIISHandler.Evidence().OrderByDescending(p => p.score).FirstOrDefault()?.passage ?? "No Passage found";
        //    return item;
        //}

        public static void RunBenchmarks()
        {
            var summary = BenchmarkRunner.Run<PostBenchMark>();
        }
    }
}
