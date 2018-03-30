using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PathFinding
{

    public class Node {

        public List<Edge> Edges { get { return edges; } }
        protected List<Edge> edges;

        // for path finding
        public float distance;
        public Node prev = null;

        public Node() {
            edges = new List<Edge>();
        }

        public Edge Connect(Node node, float weight = 1f)
        {
            var e = new Edge(this, node, weight);
            edges.Add(e);
            node.edges.Add(e);
            return e;
        }

    }

}


