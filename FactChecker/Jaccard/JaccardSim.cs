﻿using System.Collections.Generic;
using System.Linq;
using System;
using F23.StringSimilarity;
using Microsoft.VisualBasic;

namespace FactChecker.JaccardSim
{
    public class JaccardSim
    {
        //Problems with Jaccard
        //It doesn’t consider term frequency(how many times a term occurs in a document). It simply counts the number of terms that are common between two sets.
        //Rare terms in a collection are more informative than frequent terms.Jaccard doesn’t consider this information.
        //Different sized sets with same number of common members (intersection) also will result in the same Jaccard similarity.

        public double similarity(string triple, string passage)
        {
            List<string> triples = Filter(triple).Split(" ").ToList();
            List<string> passages = Filter(passage).Split(" ").ToList();
            List<string> union = new List<string>();
            List<string> intersection = new List<string>();
            union = triples.Concat(passages).ToList();
            union = union.GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            intersection = triples.Intersect(passages).ToList();

            return Math.Round((double)intersection.Count() / (double)union.Count(), 2);
        }

        public double similarity_v2(string triple, string passage)
        {
            IEnumerable<string> triples = Filter(triple).Split(" ");
            IEnumerable<string> passages = Filter(passage).Split(" ");
            IEnumerable<string> union = new string[passage.Length + triple.Length];
            IEnumerable<string> intersection = new string[passage.Length + triple.Length];
            union = triples.Union(passages);
            intersection = triples.Intersect(passages);
            return (double)MathF.Round(intersection.Count() / union.Count(), 2);
        }

        public double similarity_v3(string triple, string passage)
        {
            List<string> triples = Filter(triple).Split(" ").ToList();
            List<string> passages = Filter(passage).Split(" ").ToList();
            List<string> union = triples.Union(passages).ToList();
            List<string> intersection = triples.Intersect(passages).ToList();
            return (double)MathF.Round(intersection.Count() / union.Count(), 2);
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
