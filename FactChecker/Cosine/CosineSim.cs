using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Cryptography;
using Accord.Math;
using PEFile;
using System.Globalization;
using FactChecker.Stopwords;

namespace FactChecker.Cosine
{
    public class CosineSim
    {
        public double similarity(string triple, string passage)
        {
            triple = Filter(triple);
            passage = Filter(passage);
            List<string> tripleSplit = triple.Split(" ").ToList();
            List<string> passageSplit = passage.Split(" ").ToList();
            List<string> union = new List<string>();
            passageSplit = removeStopword(passageSplit);
            tripleSplit = removeStopword(tripleSplit);
            union = tripleSplit.Concat(passageSplit).ToList();
            union = union.GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            double[] bow1 = listToArr(tripleSplit, union).Select(p => (double)p).ToArray();
            double[] bow2 = listToArr(passageSplit, union).Select(p => (double)p).ToArray();
            double sum = Matrix.Dot(bow1, bow2);

            return Math.Round(sum / ((csNorm(bow1) * csNorm(bow2))), 2);
        }

        public double similarity_v2(string triple, string passage)
        {
            triple = Filter(triple);
            passage = Filter(passage);
            List<string> tripleSplit = triple.Split(" ").ToList();
            List<string> passageSplit = passage.Split(" ").ToList();
            List<string> union = new List<string>();
            passageSplit = removeStopword(passageSplit);
            tripleSplit = removeStopword(tripleSplit);
            union = tripleSplit.Concat(passageSplit).ToList();
            union = union.GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            double[] bow1 = listToArr(tripleSplit, union).Select(p => (double)p).ToArray();
            double[] bow2 = listToArr(passageSplit, union).Select(p => (double)p).ToArray();
            double sum = Matrix.Dot(bow1, bow2);

            return Math.Round(sum / ((csNorm(bow1) * csNorm(bow2))), 2);
        }

        public List<string> removeStopword(List<string> withStopword)
        {
            Stopwords.Stopwords sw = new();

            foreach (string removeWord in sw.stopwords_hashset)
            {
                for (int i = 0; i < withStopword.Count; i++)
                    withStopword.Remove(removeWord);
            }

            return withStopword;
        }

        public string Filter(string str)
        {
            List<char> charsToRemove = new List<char>() { '@', '_', ',', '.', ';', ':', '!', '?' };
            foreach (char c in charsToRemove)
                str = str.Replace(c.ToString(), String.Empty);

            return str;
        }

        private double csNorm(double[] vec)
        {
            double agg = 0;

            foreach (double entry in vec)
                agg += 1.0 * entry * entry;

            return Math.Sqrt(agg);
        }

        public int[] listToArr(List<string> compare, List<string> unionDic)
        {
            int[] bag = new int[unionDic.Count];
            int i = 0;
            foreach (string entry in unionDic)
            {
                foreach (string comp in compare)
                {
                    if (entry == comp)
                        bag[i] += 1;
                }
                i++;
            }

            return bag;
        }
    }
}
