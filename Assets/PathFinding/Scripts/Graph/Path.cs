using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PathFinding
{

    public class Path {

        public static readonly int SOURCE = -1;

        protected List<int> indices;
        protected int source;

        public Path(List<Node> nodes, int source)
        {
            this.source = source;

            indices = new List<int>();
            for(int i = 0, n = nodes.Count; i < n; i++)
            {
                if(i == source)
                {
                    indices.Add(SOURCE);
                } else
                {
                    var node = nodes[i];

                    // if node.prev is null,
                    // nodes.IndexOf returns -1
                    indices.Add(nodes.IndexOf(node.prev));
                }
            }
        }

        public int Prev(int index)
        {
            return indices[index];
        }

        public bool Traverse(int destination, out List<int> indices)
        {
            indices = new List<int>();
            while(destination != source) {
                indices.Add(destination);
                destination = Prev(destination);
                if (destination < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Traverse(Graph graph, int destination, out List<Node> nodes)
        {
            nodes = new List<Node>();

            List<int> indices;
            var result = Traverse(destination, out indices);
            if (!result)
            {
                return false;
            }

            nodes = indices.Select(i => graph.Nodes[i]).ToList();
            nodes.Add(graph.Nodes[source]);
            return true;
        }

        public string BuildRow()
        {
            return String.Join(",", indices.Select(i => i.ToString()).ToArray());
        }

    }

}


