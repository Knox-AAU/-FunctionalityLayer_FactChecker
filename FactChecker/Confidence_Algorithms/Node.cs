using System.Collections.Generic;

namespace FactChecker.Confidence_Algorithms 
{
    public class Node
    {
        public string data { get; set; }
        public List<Node> children = new();
        public List<Node> parents = new();
        public Node(string data)
        {
            this.data = data;
        }

        public List<Node> GetNeighbours()
        {
            List<Node> neighbours = new();
            foreach (Node node in children)
                neighbours.Add(node);

            foreach (Node node in parents)
                neighbours.Add(node);

            return neighbours;
        }
    }
}
