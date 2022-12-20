using System;

namespace FactChecker.Confidence_Algorithms
{
    public class AdjacencyMatrix
    {
        public int[,] adjMatrix { get; set; }

        public void Create(Graph g)
        {
            int nodes_ct = g.nodes.Count;
            adjMatrix = new int[nodes_ct, nodes_ct];

            for (int nodeIdx = 0; nodeIdx < nodes_ct; nodeIdx++)
                foreach (var neighbour in g.nodes[nodeIdx].GetNeighbours())
                    adjMatrix[nodeIdx, g.nodes.IndexOf(neighbour)] = 1;
        }

        public void Print(Graph g, bool showInfo = false)
        {
            for (int row = 0; row < adjMatrix.GetLength(0); row++)
            {
                if (showInfo)
                {
                    // prints node name + the indicies of its neighbours
                    Console.Write($"{g.nodes[row].data} ");
                    g.nodes[row].GetNeighbours().ForEach(o => Console.Write($"{g.nodes.IndexOf(o)} "));
                    Console.WriteLine();
                }

                // prints the entries for the node
                for (int col = 0; col < adjMatrix.GetLength(1); col++)
                    Console.Write(String.Format("{0} ", adjMatrix[row, col]));
                Console.WriteLine();
            }
        }
    }

}
