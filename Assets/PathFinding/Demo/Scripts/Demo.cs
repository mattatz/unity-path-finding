using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;

namespace PathFinding.Demo
{

    public class Demo : MonoBehaviour {

        [SerializeField] protected int width = 10, height = 10;
        [SerializeField] protected int destination = 84;

        protected const int source = 0;
        protected Graph graph;
        protected Route route;

        protected GUIStyle style = new GUIStyle();

        protected void Start()
        {
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

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
                    var node = new Node3D(new Vector3(x, 0, y) + offset + noise);
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
                        var e = node.Connect(to, Vector3.Distance((node as Node3D).Position, (to as Node3D).Position));
                        edges.Add(e);
                    }
                    if(y < height - 1)
                    {
                        var to = nodes[idx + width];
                        var e = node.Connect(to, Vector3.Distance((node as Node3D).Position, (to as Node3D).Position));
                        edges.Add(e);
                    }
                }
            }

            graph = new Graph(nodes, edges);
            route = graph.Find(source % (graph.Nodes.Count));
        }

        protected void Update()
        {
            if(Time.frameCount % 2 == 0)
            {
                destination++;
            }
        }

        protected void OnDrawGizmos()
        {
            if (graph == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;

            #if UNITY_EDITOR
            Handles.matrix = transform.localToWorldMatrix;
            #endif

            destination = Mathf.Max(0, destination % (graph.Nodes.Count));

            for(int i = 0, n = graph.Nodes.Count; i < n; i++)
            {
                var node = graph.Nodes[i] as Node3D;
                Gizmos.color = ((i == source) || (i == destination)) ? Color.white : Color.black;
                Gizmos.DrawSphere(node.Position, 0.1f);
            }

            Gizmos.color = Color.black;

            graph.Edges.ForEach(e =>
            {
                var from = e.From as Node3D;
                var to = e.To as Node3D;
                Gizmos.DrawLine(from.Position, to.Position);

                #if UNITY_EDITOR
                var distance = Vector3.Distance(Camera.main.transform.position, transform.TransformPoint(from.Position));
                style.fontSize = Mathf.FloorToInt(Mathf.Lerp(20, 8, distance * 0.075f));
                Handles.Label((from.Position + to.Position) * 0.5f, e.Weight.ToString("0.00"), style);
                #endif
            });

            Gizmos.color = Color.green;

            var nodes = route.Traverse(graph, destination);
            for(int i = 0, n = nodes.Count - 1; i < n; i++)
            {
                var from = nodes[i];
                var to = nodes[i + 1];
                Gizmos.DrawLine((from as Node3D).Position, (to as Node3D).Position);
            }

        }

    }

}


