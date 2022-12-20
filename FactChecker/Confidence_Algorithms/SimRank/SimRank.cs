using FactChecker.APIs.KnowledgeGraphAPI;
using System.Linq;

namespace FactChecker.Confidence_Algorithms.SimRank
{
    public class SimRank
    {
        public Graph Graph { get; }

        public SimRank(Graph graph)
        {

            Graph = graph;
            graph.init();
        }
        public float getSimRank(string name1, string name2, int iterations = 250, float decay_factor = 0.8f)
        {
            Similarity sim = new(Graph, decay_factor: decay_factor);

            for (int i = 0; i < iterations; i++)
                sim.SimRank_one_iter(Graph, sim.old_sim);

            return sim.get_sim_value(name1, name2);
        }

        public float GetSimRank(KnowledgeGraphItem item, int iterations = 250, float decay_factor = 0.8f)
        {
            float res = 0f;
            res += getSimRank(item.s, item.t, iterations, decay_factor);
            return res;
        }

        public float GetSimRank(MultipleKnowledgeGraphItem items, int iterations = 250, float decay_factor = 0.8f) =>
            items.Items.Average(p => GetSimRank(p, iterations, decay_factor)) * 100;
    }
}
