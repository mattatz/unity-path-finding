using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Graph {

        public List<Node> Nodes { get { return nodes; } }
        public List<Edge> Edges { get { return edges; } }

        protected List<Node> nodes;
        protected List<Edge> edges;

        public Graph(List<Node> ns, List<Edge> es)
        {
            nodes = ns;
            edges = es;
        }

    }

}


