using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.BenchMarks;
using System;
using System.Collections.Generic;

namespace FactChecker.JaccardSim
{
    [InProcess]
    [MemoryDiagnoser]
    public class JaccardBenchmark
    {
        [ParamsSource(nameof(Targets))]
        public string Target { get; set; }
        [ParamsSource(nameof(Sources))]
        public string Source { get; set; }


        public IEnumerable<string> Targets => new[]
        {
            "build",
            "Holland instance of Spider-Man",
        };
        public IEnumerable<string> Sources => new[]
        {
            "This repo contains LLILC, an LLVM based compiler for .NET Core. It includes a set of cross-platform .NET code generation tools that enables compilation of MSIL byte code to LLVM supported platforms.",
            "the Ioniq 5 electric-powered SUV, starring Holland and Batalon, with Watts directing. The Ioniq 5 and Hyundai Tucson are featured in the film. In late February 2021, Holland, Batalon, and Zendaya released three stills featuring their characters from the film alongside fake logos with the titles Spider-Man: Phone Home, Spider-Man: Home-Wrecker, and Spider-Man: Home Slice, respectively. The film's official title was announced the next day with a video showing Holland, Batalon, and Zendaya leaving Watts' office (where they supposedly received"
        };

        [Benchmark]
        public void BenchJaccard()
        {
            JaccardSim js = new();
            js.similarity(Target, Source);
        }

        [Benchmark]
        public void BenchJaccard_v2()
        {
            JaccardSim js = new();
            js.similarity_v2(Target, Source);
        }

        [Benchmark]
        public void BenchJaccard_v3()
        {
            JaccardSim js = new();
            js.similarity_v3(Target, Source);
        }

        public static void RunBenchmarks()
        {
            var summary = BenchmarkRunner.Run<JaccardSim>();
        }
    }
}
