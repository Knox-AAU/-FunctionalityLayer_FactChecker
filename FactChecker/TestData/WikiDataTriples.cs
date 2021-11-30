using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TestData
{

    public class WikiDataTriples
    {
        public List<string> triples = new List<string>();


        public WikiDataTriples()
        {
            ReadTriplesFromFile();
        }

        public async void ReadTriplesFromFile ()
        {
            IO.FileStreamHandler fileStreamHandler = new IO.FileStreamHandler();
            foreach (string s in await fileStreamHandler.ReadFile("./TestData/relations.txt"))
            {
                triples.Add(s);
            }
        }
    }
}
