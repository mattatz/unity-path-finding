using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    public class Dijkstra {

        public static Path Find(Graph graph, int source)
        {
            var queue = new PriorityQueue<float, Node>();

            for(int i = 0, n = graph.Nodes.Count; i < n; i++)
            {
                var node = graph.Nodes[i];
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

            return new Path(graph.Nodes, source);
        }

        public static Path Find(Graph graph, Node node)
        {
            var index = graph.Nodes.IndexOf(node);
            return Find(graph, index);
        }

    }

}


