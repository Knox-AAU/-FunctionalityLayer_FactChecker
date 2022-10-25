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
            //Levenshtein.LevenshteinBenchmarks.RunBenchmarks();
            Rake.RakeBenchmark.RunBenchmarks();
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
