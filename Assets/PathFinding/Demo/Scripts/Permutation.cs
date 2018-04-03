using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;

namespace PathFinding.Demo
{

    public class Permutation : MonoBehaviour {

        [SerializeField] protected int width = 10, height = 10;

        protected Graph graph;
        protected List<Path> routes;

        protected void Start()
        {
            var nodes = new List<Node>();
            var edges = new List<Edge>();

            var half = - Vector3.one * 0.5f;
            var offset = - new Vector3(
                width - ((width % 2 == 0) ? 1f : 0f),
                0f, 
                height - ((height % 2 == 0) ? 1f : 0f)
            ) * 0.5f;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    var noise = new Vector3(Random.value, Random.value, Random.value) + half;
                    var node = new Node(new Vector3(x, 0, y) + offset + noise);
                    nodes.Add(node);
                }
            }

            for(int y = 0; y < height; y++)
            {
                var yoff = y * width;
                for(int x = 0; x < width; x++)
                {
                    var idx = yoff + x;
                    var node = nodes[idx];
                    if(x < width - 1)
                    {
                        var to = nodes[idx + 1];
                        var e = node.Connect(to, Vector3.Distance(node.Position, to.Position));
                        edges.Add(e);
                    }
                    if(y < height - 1)
                    {
                        var to = nodes[idx + width];
                        var e = node.Connect(to, Vector3.Distance(node.Position, to.Position));
                        edges.Add(e);
                    }
                }
            }

            graph = new Graph(nodes, edges);
            routes = graph.Permutation();
        }

        protected void Update()
        {
        }

        protected void OnDrawGizmos()
        {
        }

    }

}


