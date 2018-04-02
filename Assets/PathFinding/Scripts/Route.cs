using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PathFinding
{

    public class Route {

        public static readonly int SOURCE = -1;

        protected List<int> indices;
        protected int source;

        public Route(List<Node> nodes, int source)
        {
            indices = new List<int>();
            for(int i = 0, n = nodes.Count; i < n; i++)
            {
                if(i == source)
                {
                    indices.Add(SOURCE);
                } else
                {
                    var node = nodes[i];
                    indices.Add(nodes.IndexOf(node.prev));
                }
            }
        }

        public int Prev(int index)
        {
            return indices[index];
        }

        public List<int> Traverse(int destination)
        {
            var path = new List<int>();
            while(destination != source) {
                path.Add(destination);
                destination = Prev(destination);
            }
            return path;
        }

        public List<Node> Traverse(Graph graph, int destination)
        {
            var path = Traverse(destination);
            var nodes = path.Select(i => graph.Nodes[i]).ToList();
            nodes.Add(graph.Nodes[source]);
            return nodes;
        }

        public string BuildRow()
        {
            return String.Join(",", indices.Select(i => i.ToString()).ToArray());
        }

    }

}


