using FactChecker.APIs.KnowledgeGraphAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.Confidence_Algorithms
{
    public class Katz
    {
        public Graph Graph { get; }

        public Katz(Graph graph)
        {

            Graph = graph;
            graph.init();
        }
        public float ComputeCentrality(MultipleKnowledgeGraphItem items)
        {
            Dictionary<string, float> centralityVector = Centrality(Graph);

            float res = 0;

            foreach (var item in items.Items)
            {
                res += centralityVector[item.s];
                res += centralityVector[item.t];
            }

            res = res / (items.Items.Count*2); // finds the average of the queries instead

            return res;
        }

        public Dictionary<string, float>? Centrality(Graph g, float alpha = 0.1f, float beta = 1.0f, float max_iter = 1000, float tol = (float)1.0e-6, Dictionary<string, float>? nstart = null, bool normalized = true)
        {
            if (g.nodes.Count() == 0) return null;

            AdjacencyMatrix A = new();
            A.Create(g);

            // initialize starting vector x
            Dictionary<string, float>? x = new();

            if (nstart == null)
                g.nodes.ForEach(node => x.Add(node.data, 0));
            else
                x = nstart;


            // initialize b
            Dictionary<string, float>? b = new();
            g.nodes.ForEach(node => b.Add(node.data, (float)beta));

            for (int i = 0; i < max_iter; i++)
            {
                Dictionary<string, float>? xlast = x.ToDictionary(x => x.Key, x => x.Value);
                x.ToDictionary(a => a.Key, a => 0); // sets x entries to 0

                foreach (var n in x)
                {
                    int n_idx = x.Keys.ToList().IndexOf(n.Key);
                    List<Node> nbrs = g.nodes[n_idx].GetNeighbours();
                    foreach (var nbr in nbrs)
                    {
                        int nbr_idx = x.Keys.ToList().IndexOf(nbr.data);
                        x[nbr.data] += xlast[n.Key] * A.adjMatrix[n_idx, nbr_idx];
                    }
                }

                foreach (var n in x)
                    x[n.Key] = alpha * x[n.Key] + b[n.Key];

                float err = x.Sum(n => Math.Abs(x[n.Key] - xlast[n.Key]));
                if (err < g.nodes.Count * tol)
                {
                    float s = 0;
                    if (normalized)
                    {
                        try
                        {
                            s = 1.0f / MathF.Sqrt(x.Sum(v => MathF.Pow(v.Value, 2)));
                        }
                        catch (DivideByZeroException)
                        {
                            s = 1.0f;
                        }
                    }
                    else
                    {
                        s = 1;
                    }
                    foreach (var n in x)
                        x[n.Key] *= s;
                    return x;
                }
            }
            throw new Exception(message: $"Power iteration failed");
        }
    }
}
