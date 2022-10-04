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
            triple = Filter_v2(triple);
            passage = Filter_v2(passage);
            List<string> tripleSplit = triple.Split(" ").ToList();
            List<string> passageSplit = passage.Split(" ").ToList();
            List<string> union = new List<string>();
            passageSplit = removeStopword_v2(passageSplit);
            tripleSplit = removeStopword_v2(tripleSplit);
            union = tripleSplit.Concat(passageSplit).ToList();
            union = union.GroupBy(p => p).Select(p => p.First()).Distinct().ToList();
            double[] bow1 = listToArr_v2(tripleSplit, union);
            double[] bow2 = listToArr_v2(passageSplit, union);
            double sum = Matrix.Dot(bow1, bow2);

            return Math.Round(sum / ((csNorm_v2(bow1) * csNorm_v2(bow2))), 2);
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

        public List<string> removeStopword_v2(List<string> withStopword)
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

        public string Filter_v2(string str)
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
        private double csNorm_v2(double[] vec)
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
        public double[] listToArr_v2(List<string> compare, List<string> unionDic)
        {
            double[] bag = new double[unionDic.Count];
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
