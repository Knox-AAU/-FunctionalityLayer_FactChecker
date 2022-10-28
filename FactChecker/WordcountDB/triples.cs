using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactChecker.WordcountDB
{
    public class TripleItem
    {
        public int id { get; set; }
        public string s { get; set; }
        public string r { get; set; }
        public string t { get; set; }
        public TripleItem()
        {
              
        }

        public TripleItem(string s, string r, string t)
        {
            this.s = s;
            this.r = r;
            this.t = t;
        }
    }
    public class triples
    {
        private readonly KnoxFactCheckingTestDbContext context;
        public triples(KnoxFactCheckingTestDbContext context)
        {
            this.context = context;
        }

        public List<TripleItem> GetAll()
        {
            return context.triples.ToList();
        }
        public List<KnowledgeGraphItem> GetAllAsKnowledgeGraphItem()
        {
            return context.triples.ToList().Select(p => new KnowledgeGraphItem(p.s, p.r, p.t)).ToList();
        }


        public void UploadAllRelations()
        {
            foreach (var item in getTriplesFromPath())
            {
                if (context.triples.FirstOrDefault(p => p.t == item.t && p.r == item.r && p.s == item.s) == null)
                    context.triples.Add(item);
            }
            context.SaveChanges();
        }

        private List<TripleItem> getTriplesFromPath()
        {
            List<TripleItem> strings = new();
            foreach (string triple_str in File.ReadLines(Path.GetFullPath("TestData/relations.txt")))
            {
                string[] splitTriple = triple_str.Split("> <");
                TripleItem t = new(splitTriple[0].TrimStart('<'), splitTriple[1], splitTriple[2].TrimEnd('>'));
                strings.Add(t);
            }
            return strings;
        }

    }
}
