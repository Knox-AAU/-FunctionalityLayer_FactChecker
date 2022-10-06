using System.Collections.Generic;

namespace FactChecker.SimRank
{
    public partial class SimRank
    {
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


    }



}
