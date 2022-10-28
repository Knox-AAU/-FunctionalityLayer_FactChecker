using FactChecker.APIs.KnowledgeGraphAPI;
using System.Linq;

namespace FactChecker.SimRank
{
    public class SimRank
    {
    //    public float getSimRank(string name1, string name2, int iterations = 250, float decay_factor = 0.8f)
    //    {
    //        Graph graph;
    //        graph.init();

    //        Similarity sim = new(graph, decay_factor: decay_factor);

    //        for (int i = 0; i < iterations; i++)
    //            sim.SimRank_one_iter(graph, sim.old_sim);

    //        //sim.Print_Sim();
    //        //graph.Print_Nodes();

    //        return sim.get_sim_value(name1, name2);
    //    }

    //    public float GetSimRank(KnowledgeGraphItem item, int iterations = 250, float decay_factor = 0.8f)
    //    {
    //        float res = 0f;
    //        res += getSimRank(item.s, item.t, iterations, decay_factor);
    //        return res;
    //    }

    //    public float GetSimRank(MultipleKnowledgeGraphItem items, int iterations = 250, float decay_factor = 0.8f) => 
    //        items.Items.Average(p => GetSimRank(p, iterations, decay_factor)) * 100;
    }
}
