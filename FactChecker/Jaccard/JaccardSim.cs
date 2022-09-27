using System.Collections.Generic;
using System.Linq;
using System;

namespace FactChecker.Jaccard
{
    public class JaccardSim
    {
        public double similarity(string triple, string passage)
        {
            List<string> triples = triple.Split(" ").ToList();
            List<string> passages = passage.Split(" ").ToList();
            List<string> union = new List<string>();
            List<string> intersection = new List<string>();
            union = triples.Concat(passages).ToList();
            union = union.GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            intersection = triples.Intersect(passages).ToList();

            return Math.Round((double)intersection.Count() / (double)union.Count(), 2);
        }
    }
}
