using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PathFinding.Demo
{

    public class AnimationPathDemo : MonoBehaviour
    {

        [SerializeField] protected int width = 15, height = 15;
        [SerializeField] protected int source = 0;
        [SerializeField] protected int length = 30;
        [SerializeField] protected int count = 100;

        [SerializeField] protected NodeRenderer nodeRenderer;
        [SerializeField] protected PathRenderer pathRenderer;

        protected Graph graph;
        protected Path path;
        protected PathBundle bundle;

        void Start()
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
            path = graph.Find(source % (graph.Nodes.Count));

            count = Mathf.Min(count, graph.Nodes.Count);

            bundle = new PathBundle(length);

            for(int i = 0; i < count; i++)
            {
                var idx = (source + (i + 1)) % graph.Nodes.Count;
                var route = path.Traverse(graph, idx);
                if(route.Count > 10)
                {
                    var points = route.Select(n => n.Position).ToList();
                    bundle.Add(points);
                }
            }

            nodeRenderer.Setup(graph);
            pathRenderer.Setup(bundle);
        }

        protected void OnDrawGizmosSelected()
        {
            if (bundle == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.white;

            bundle.pathes.ForEach(path =>
            {
                for(int i = 0, n = path.Count - 1; i < n; i++)
                {
                    var p0 = path[i];
                    var p1 = path[i + 1];
                    Gizmos.DrawLine(p0, p1);
                }
            });
        }

    }

}


