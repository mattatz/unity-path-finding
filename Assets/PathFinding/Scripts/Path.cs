using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Path {

        public static readonly int SOURCE = -1;

        public List<int> Route { get { return route; } }
        public int Source { get { return source; } }

        protected List<int> route;
        protected int source;

        public Path(List<Node> nodes, int source)
        {
            route = new List<int>();
            for(int i = 0, n = nodes.Count; i < n; i++)
            {
                if(i == source)
                {
                    route.Add(SOURCE);
                } else
                {
                    var node = nodes[i];
                    route.Add(nodes.IndexOf(node.prev));
                }
            }
        }

    }

}


