using System.Collections.Generic;

namespace FactChecker.SimRank
{
    public partial class SimRank
    {
        public float getSimRank(string name1, string name2, int iterations=250, float decay_factor=0.9f)
        {
            Graph graph = new Graph();
            graph.init();

            Similarity sim = new(graph, decay_factor: decay_factor);

            for (int i = 0; i < iterations; i++)
                sim.SimRank_one_iter(graph, sim.old_sim);

            //sim.Print_Sim();
            //graph.Print_Nodes();

            return sim.get_sim_value(name1, name2);
        }
    }
}
