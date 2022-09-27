using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.TMWIIS;
using FactChecker.BenchMarks;

namespace FactChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //Console.WriteLine(Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance_V1("hello there dawpdjapw djpaowd ", "this is a test"));
            //Console.WriteLine(Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance_V2("hello there dawpdjapw djpaowd ", "this is a test"));
            //Console.WriteLine(Levenshtein.LevenshteinDistanceAlgorithm.LevenshteinDistance_V3("hello there dawpdjapw djpaowd ", "this is a test"));

            Levenshtein.LevenshteinBenchmarks.RunBenchmarks();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
