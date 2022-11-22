using FactChecker.APIs.KnowledgeGraphAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactChecker.Confidence_Algorithms
{
    public class AdamicAdar
    {
        public float CalculateScore(MultipleKnowledgeGraphItem items)
        {
            Graph graph = new();
            graph.init();

            float res = 0;

            foreach (var item in items.Items)
            {
                Node a = graph?.nodes?.Find(o => o.data == item.s);
                Node b = graph?.nodes?.Find(o => o.data == item.t);
                res += Calculate(a, b);
            }

            res = res / items.Items.Count; // finds the average of the queries instead

            return res;
        }

        public float Calculate(Node x, Node y)
        {
            float sum = 0;
            List<Node> u = Intersection(x.GetNeighbours(), y.GetNeighbours());

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
