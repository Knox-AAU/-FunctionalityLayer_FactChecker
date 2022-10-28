using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.BenchMarks;
using System;
using System.Collections.Generic;

namespace FactChecker.Cosine
{
    [InProcess]
    [MemoryDiagnoser]
    public class CosineBench
    {
        private string __checkstring_target_long = "Holland instance of Spider-Man";
        private string __checkstring_source_long = "the Ioniq 5 electric-powered SUV, starring Holland and Batalon, with Watts directing. The Ioniq 5 and Hyundai Tucson are featured in the film. In late February 2021, Holland, Batalon, and Zendaya released three stills featuring their characters from the film alongside fake logos with the titles Spider-Man: Phone Home, Spider-Man: Home-Wrecker, and Spider-Man: Home Slice, respectively. The film's official title was announced the next day with a video showing Holland, Batalon, and Zendaya leaving Watts' office (where they supposedly received";

        private string __checkstring_target_short = "This repo contains LLILC, an LLVM based compiler for .NET Core. It includes a set of cross-platform .NET code generation tools that enables compilation of MSIL byte code to LLVM supported platforms.";
        private string __checkstring_source_short = "build";
        
        List<string> myStopwords = new() { "a", "about", "above", "after", "again", "against", "all", "am", "and", "he", "the", "them", "these", "to", "too", "who", "your", "why", "which", "me", "if", "most" };
        //[Benchmark]
        //public void BenchCosine_long()
        //{
        //    CosineSim cs = new();
        //    cs.similarity(__checkstring_target_long, __checkstring_source_long);
        //}

        //[Benchmark]
        //public void BenchCosine_short()
        //{
        //    CosineSim cs = new();
        //    cs.similarity(__checkstring_target_short, __checkstring_source_short);
        //}

        //[Benchmark]
        //public void BenchCosine_v2_long()
        //{
        //    CosineSim cs = new();
        //    cs.similarity_v2(__checkstring_target_long, __checkstring_source_long);
        //}

        //[Benchmark]
        //public void BenchCosine_v2_short()
        //{
        //    CosineSim cs = new();
        //    cs.similarity_v2(__checkstring_target_short, __checkstring_source_short);
        //}


        public static void RunBenchmarks()
        {
            var summary = BenchmarkRunner.Run<CosineSim>();
        }
    }
}
