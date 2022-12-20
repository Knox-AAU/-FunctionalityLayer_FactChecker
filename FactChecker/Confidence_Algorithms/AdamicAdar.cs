using FactChecker.APIs.KnowledgeGraphAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.Confidence_Algorithms
{
    public class AdamicAdar
    {
        public Graph Graph { get; }

        public AdamicAdar(Graph graph)
        {

            Graph = graph;
            graph.init();
        }
        /*
        public float GetAdamicAdar(string name1, string name2)
        {
            return Calculate(Graph.nodes.Where(n => n.data == name1), Graph.nodes.Where(n => n.data == name2));
        }

        public float GetAdamicAdar(KnowledgeGraphItem item)
        {
            float res = 0f;
            res += GetAdamicAdar(item.s, item.t);
            return res;
        }
        */
        public float CalculateScore(MultipleKnowledgeGraphItem items)
        {

            float res = 0;

            foreach (var item in items.Items)
            {
                Node a = Graph?.nodes?.Find(o => o.data == item.s);
                Node b = Graph?.nodes?.Find(o => o.data == item.t);
                res += Calculate(a, b);
            }

            res = res / items.Items.Count; // finds the average of the queries instead

            return res;
        }

        public float Calculate(Node x, Node y)
        {
            float sum = 0;
            List<Node> u = Intersection(x.GetNeighbours(), y.GetNeighbours());

            if (u.Count <= 1) return 0;

            foreach (Node node in u)
                sum += 1 / MathF.Log10(u.Count);

            return sum;
        }

        public List<Node> Intersection(List<Node> a, List<Node> b)
        {
            List<Node> res = new();

            foreach (Node node in a)
                foreach (Node node2 in b)
                    if (node.data == node2.data)
                        res.Add(node);
            return res;
        }
    }
}
