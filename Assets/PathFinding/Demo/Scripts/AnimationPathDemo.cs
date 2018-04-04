using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PathFinding.Demo
{

    public class AnimationPathDemo : MonoBehaviour
    {

        [SerializeField] protected int width = 15, height = 5, depth = 15;
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
                height - ((height % 2 == 0) ? 1f : 0f),
                depth - ((depth % 2 == 0) ? 1f : 0f)
            ) * 0.5f;

            for(int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var noise = new Vector3(Random.value, Random.value, Random.value) + half;
                        var node = new Node(new Vector3(x, y, z) + offset + noise * 0.5f);
                        nodes.Add(node);
                    }
                }
            }

            for(int z = 0; z < depth; z++)
            {
                var zoff = z * (width * height);
                for (int y = 0; y < height; y++)
                {
                    var yoff = y * width;
                    for (int x = 0; x < width; x++)
                    {
                        var idx = zoff + yoff + x;
                        var node = nodes[idx];
                        if (x < width - 1)
                        {
                            var to = nodes[idx + 1];
                            var e = node.Connect(to, Vector3.Distance(node.Position, to.Position));
                            edges.Add(e);
                        }
                        if (y < height - 1)
                        {
                            var to = nodes[idx + width];
                            var e = node.Connect(to, Vector3.Distance(node.Position, to.Position));
                            edges.Add(e);
                        }
                        if (z < depth - 1)
                        {
                            var to = nodes[idx + width * height];
                            var e = node.Connect(to, Vector3.Distance(node.Position, to.Position));
                            edges.Add(e);
                        }
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
                    route.Reverse();
                    var points = route.Select(n => n.Position).ToList();
                    bundle.Add(points);
                }
            }

            nodeRenderer.Setup(graph);
            pathRenderer.Setup(bundle);
        }

        protected void OnDrawGizmosSelected()
        {
            if (graph == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            graph.Edges.ForEach(e =>
            {
                var p0 = e.From.Position;
                var p1 = e.To.Position;
                Gizmos.DrawLine(p0, p1);
            });


        }

    }

}


