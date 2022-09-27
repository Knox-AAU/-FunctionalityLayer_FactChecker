using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            //RunBenchmarks();
            main();
        }
        public static void main()
        {
            Graph graph = new Graph();

            int iteration = 500;
            float decay_factor = 0.9f;

            graph.init();
            Similarity sim = new(graph, decay_factor: decay_factor);

            for (int i = 0; i < iteration; i++)
                sim.SimRank_one_iter(graph, sim.old_sim);

            sim.Print_Sim();
            //graph.Print_Nodes();
        }
        [MemoryDiagnoser]
        public class BM
        {
            [Benchmark]
            public void Test1()
            {
                main();
            }
        }
        public static void RunBenchmarks()
        {
            BenchmarkRunner.Run<BM>();
        }
        class Node
        {
            public string data { get; set; }
            public List<Node> children = new();
            public List<Node> parents = new();
            public Node(string data)
            {
                this.data = data;
            }
        }
        class Triple
        {
            public string S { get; set; }
            public string R { get; set; }
            public string T { get; set; }

            public Triple(string s, string r, string t)
            {
                S = s;
                R = r;
                T = t;
            }

        }
        class Similarity
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
                
                foreach(Node n in nodes)
                    name_list.Add(n.data);
                
                List<List<float>> sim = new();

                foreach(string name1 in name_list)
                {
                    List<float> temp_sim = new();
                    foreach(string name2 in name_list)
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
                foreach(Node node1 in graph.nodes)
                {
                    foreach(Node node2 in graph.nodes)
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
            private float Calculate_SimRank(Node node1, Node node2)
            {
                if (node1.data == node2.data)
                    return 1.0f;

                List<Node> in_neighbours1 = node1.parents;
                List<Node> in_neighbours2 = node2.parents;

                if(in_neighbours1.Count == 0 || in_neighbours2.Count == 0)
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
        class Graph
        {
            public List<Node> nodes = new();
            private List<Triple> triples = new();

            public List<List<float>> old_sim = new();

            public void init()
            {
                getTriples();

                foreach (Triple triple in triples)
                {
                    Node a = nodes.FirstOrDefault(o => o.data == triple.S) ?? init_node(triple.S);
                    Node b = nodes.FirstOrDefault(o => o.data == triple.T) ?? init_node(triple.T);
                    
                    if (!a.children.Any(o => o.data == triple.T))
                    {
                        nodes.First(o => o.data == a.data).children.Add(b);
                        nodes.First(o => o.data == a.data).parents.Add(b);
                    }

                    if (!b.parents.Any(o => o.data == triple.S))
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

            private void getTriples()
            {
                foreach (string line in System.IO.File.ReadLines(@"D:\FunctionalityLayer_FactChecker\SimRank\SimRank\ConsoleApp1\small_sample.txt"))
                {
                    String[] splitTriple = line.Split("> <");
                    Triple t = new(splitTriple[0].TrimStart('<'), splitTriple[1], splitTriple[2].TrimEnd('>'));
                    triples.Add(t);
                    //System.Console.WriteLine(line);
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

}