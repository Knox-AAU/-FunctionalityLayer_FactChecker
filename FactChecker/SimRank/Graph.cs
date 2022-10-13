using FactChecker.APIs.KnowledgeGraphAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactChecker.SimRank
{
        public class Graph
        {
            public List<Node> nodes = new();
            private List<KnowledgeGraphItem> triples = new();
            public List<List<float>> old_sim = new();

            public void init()
            {
                getTriplesFromPath();

                foreach (KnowledgeGraphItem triple in triples)
                {
                    Node a = nodes.FirstOrDefault(o => o.data == triple.s) ?? init_node(triple.s);
                    Node b = nodes.FirstOrDefault(o => o.data == triple.t) ?? init_node(triple.t);

                    if (!a.children.Any(o => o.data == triple.t))
                    {
                        nodes.First(o => o.data == a.data).children.Add(b);
                        nodes.First(o => o.data == a.data).parents.Add(b);
                    }

                    if (!b.parents.Any(o => o.data == triple.s))
                    {
                        nodes.First(o => o.data == b.data).parents.Add(a);
                        nodes.First(o => o.data == b.data).children.Add(a);
                    }
                }

                Node init_node(string input)
                {
                    Node a = new(input);
                    nodes.Add(a);
                    return a;
                }
            }

            private void getTriplesFromPath()
            {
                foreach (string triple_str in File.ReadLines(Path.GetFullPath("TestData/relations.txt")))
                {
                    String[] splitTriple = triple_str.Split("> <");
                    KnowledgeGraphItem t = new(splitTriple[0].TrimStart('<'), splitTriple[1], splitTriple[2].TrimEnd('>'));
                    triples.Add(t);
                }
            }

            public void Print_Nodes()
            {
                foreach (Node n in nodes)
                {
                    Console.Write($"Node: {n.data} | parent:");
                    foreach (Node p in n.parents)
                        Console.Write($" {p.data} ");

                    Console.Write(" | child:");
                    foreach (Node c in n.children)
                        Console.Write($" {c.data} ");

                    Console.WriteLine();
                }
            }

    }
}
