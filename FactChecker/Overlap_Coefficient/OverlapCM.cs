using System.Collections.Generic;
using System;
using System.Linq;

namespace FactChecker.Overlap_Coefficient
{
    public class OverlapCM
    {
        public double Similarity(string triple, string passage)
        {
            List<string> triples = Filter(triple).Split(" ").ToList();
            List<string> passages = Filter(passage).Split(" ").ToList();
            List<string> intersection = triples.Intersect(passages).ToList();

            return Math.Round(intersection.Count() / (double)Math.Min(triples.Count, passages.Count), 2);
        }

        private string Filter(string str)
        {
            List<char> charsToRemove = new List<char>() { '@', '_', ',', '.', ';', ':', '!', '?' };
            foreach (char c in charsToRemove)
                str = str.Replace(c.ToString(), String.Empty);

            return str;
        }
    }
}
