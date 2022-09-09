using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TestData
{

    public class WikiDataTriples
    {
        public List<APIs.KnowledgeGraphAPI.KnowledgeGraphItem> triples = new List<APIs.KnowledgeGraphAPI.KnowledgeGraphItem>();


        public WikiDataTriples()
        {
            ReadTriplesFromFile();
        }

        public async void ReadTriplesFromFile ()
        {
            IO.FileStreamHandler fileStreamHandler = new IO.FileStreamHandler();
            foreach (string s in await fileStreamHandler.ReadFile("./TestData/relations.txt"))
            {
               List<string> splitted = s.Split("<").ToList();
               APIs.KnowledgeGraphAPI.KnowledgeGraphItem item = new APIs.KnowledgeGraphAPI.KnowledgeGraphItem();
               for(int i = 0; i < splitted.Count; i++)
                {
                    string split = splitted[i].Replace(">", string.Empty);
                    if (i == 1)
                    {
                        item.s = split.Remove(split.Length - 1,1);
                    }
                    else if (i == 2)
                    {
                        item.r = split.Remove(split.Length - 1, 1);
                    }
                    else if (i == 3)
                    {
                        item.t = split;
                    }
                }
                triples.Add(item);
            }
        }
    }
}
