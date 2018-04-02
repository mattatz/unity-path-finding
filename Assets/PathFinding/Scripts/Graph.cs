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

        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        // Shortest path finding with Dijkstra's algorithm
        public Route Find(int source)
        {
            var queue = new PriorityQueue<float, Node>();

            for(int i = 0, n = nodes.Count; i < n; i++)
            {
                var node = nodes[i];
                node.distance = (i != source) ? float.PositiveInfinity : 0f;
                node.prev = null;
                queue.Push(node.distance, node);
            }

            while(queue.Count > 0)
            {
                var pair = queue.Pop();
                var u = pair.Value;
                u.Edges.ForEach(e =>
                {
                    var v = e.Neighbor(u);
                    var alt = u.distance + e.Weight;
                    if(alt < v.distance)
                    {
                        v.distance = alt;
                        v.prev = u;
                        queue.Remove(v);
                        queue.Push(v.distance, v);
                    }
                });
            }

            return new Route(nodes, source);
        }

        public Route Find(Node node)
        {
            var index = nodes.IndexOf(node);
            return Find(index);
        }

    }

}


