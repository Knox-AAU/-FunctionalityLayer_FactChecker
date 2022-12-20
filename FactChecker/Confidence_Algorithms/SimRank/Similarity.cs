using FactChecker.Confidence_Algorithms;
using System;
using System.Collections.Generic;

namespace FactChecker.Confidence_Algorithms.SimRank
{
    public class Similarity
    {
        public float decay_factor { get; set; }
        public List<string> name_list = new();
        public int node_num { get; set; }
        public List<List<float>> old_sim = new();
        public List<List<float>> new_sim = new();

        public Similarity(Graph graph, float decay_factor)
        {
            this.decay_factor = decay_factor;
            (name_list, old_sim) = Init_sim(graph);
            node_num = name_list.Count;

            for (int col = 0; col < node_num; col++)
            {
                List<float> temp_row = new();

                for (int row = 0; row < node_num; row++)
                    temp_row.Add(0);

                new_sim.Add(temp_row);
            }
        }

        public (List<string>, List<List<float>>) Init_sim(Graph graph)
        {
            List<Node> nodes = graph.nodes;

            List<string> name_list = new();

            foreach (Node n in nodes)
                name_list.Add(n.data);

            List<List<float>> sim = new();

            foreach (string name1 in name_list)
            {
                List<float> temp_sim = new();
                foreach (string name2 in name_list)
                {
                    if (name1 == name2)
                        temp_sim.Add(1);
                    else
                        temp_sim.Add(0);
                }

                sim.Add(temp_sim);
            }

            return (name_list, sim);
        }

        public void SimRank_one_iter(Graph graph, List<List<float>> sim)
        {
            foreach (Node node1 in graph.nodes)
            {
                foreach (Node node2 in graph.nodes)
                {
                    float new_SimRank = Calculate_SimRank(node1, node2);
                    new_sim[name_list.IndexOf(node1.data)][name_list.IndexOf(node2.data)] = new_SimRank; // updates simrank for one entry
                }
            }
            for (int i = 0; i < new_sim.Count; i++)
                for (int j = 0; j < new_sim.Count; j++)
                    old_sim[i][j] = new_sim[i][j];
        }
        private float get_sim_value(Node node1, Node node2)
        {
            int node1_idx = name_list.IndexOf(node1.data);
            int node2_idx = name_list.IndexOf(node2.data);
            return old_sim[node1_idx][node2_idx];
        }
        public float get_sim_value(string name1, string name2)
        {
            int idx1 = name_list.IndexOf(name1);
            int idx2 = name_list.IndexOf(name2);
            if (idx1 == -1 || idx2 == -1) return 0f;
            return old_sim[idx1][idx2];
        }
        private float Calculate_SimRank(Node node1, Node node2)
        {
            if (node1.data == node2.data)
                return 1.0f;

            List<Node> in_neighbours1 = node1.parents;
            List<Node> in_neighbours2 = node2.parents;

            if (in_neighbours1.Count == 0 || in_neighbours2.Count == 0)
                return 0.0f;

            float SimRank_sum = 0;

            foreach (Node n1 in in_neighbours1)
                foreach (Node n2 in in_neighbours2)
                    SimRank_sum += get_sim_value(n1, n2);

            float scale = decay_factor / (in_neighbours1.Count * in_neighbours2.Count);
            float new_SimRank = scale * SimRank_sum;
            return new_SimRank;
        }
        public void Print_Sim()
        {
            foreach (string n in name_list)
            {
                string name = (n.Length > 5) ? n[..5] : n;
                Console.Write(name);
                int max_print_len = 7 - name.ToString().Length;
                for (int i = 0; i < max_print_len; i++)
                    Console.Write("-");
            }
            Console.WriteLine();
            foreach (var row in old_sim)
            {
                foreach (float elem in row)
                {
                    float rounded = MathF.Round(elem, 3);
                    Console.Write(rounded);

                    int max_print_len = 7 - rounded.ToString().Length;
                    for (int i = 0; i < max_print_len; i++)
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }



}
