using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Edge {

        public Node From { get { return from; } }
        public Node To { get { return to; } }
        public float Weight { get { return weight; } }

        protected Node from, to;
        protected float weight = 0f;

        public Edge (Node n0, Node n1, float w = 1f)
        {
            from = n0;
            to = n1;
            weight = w;
        }

        public Node Neighbor(Node node)
        {
            if (from == node) return to;
            return from;
        }

        public bool Has(Node node)
        {
            return (from == node) || (to == node);
        }

    }

}


