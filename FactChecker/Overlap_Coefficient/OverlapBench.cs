using BenchmarkDotNet.Attributes;
using FactChecker.Cosine;
using System.Collections.Generic;

namespace FactChecker.Overlap_Coefficient
{
    [InProcess]
    [MemoryDiagnoser]
    public class OverlapBench
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
        public void BenchOverlap_long()
        {
            OverlapCM oc = new();
            oc.Similarity(Target, Source);
        }
    }
}
