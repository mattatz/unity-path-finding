using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.Test
{

    [ExecuteInEditMode]
    public class PathMeshTest : MonoBehaviour {

        [SerializeField] protected float thickness = 0.1f;
        [SerializeField] protected List<Vector3> points;
        
        void Update () {
            var cam = Camera.main;
            var camDir = cam.transform.forward;
            var localCamDir = transform.InverseTransformDirection(camDir);
            var filter = GetComponent<MeshFilter>();
            filter.sharedMesh = Build(points, localCamDir);
        }

        protected Mesh Build(List<Vector3> points, Vector3 camDir)
        {
            var mesh = new Mesh();

            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();

            var count = points.Count;

            float inv = 1f / (count - 1);

            // add tail
            var dir = (points[1] - points[0]).normalized;
            var right = Vector3.Cross(dir, camDir).normalized * 0.5f * thickness;
            vertices.Add(points[0] - right); vertices.Add(points[0] + right);
            uvs.Add(new Vector2(0f, 0f)); uvs.Add(new Vector2(1f, 0f));

            for(int i = 1; i < count - 1; i++)
            {
                var prev = points[i - 1];
                var cur = points[i];
                var next = points[i + 1];
                var dir10 = (cur - prev).normalized;
                var dir21 = (next - cur).normalized;
                dir = ((dir10 + dir21) * 0.5f);
                float dot = Mathf.Clamp01((Vector3.Dot(dir10, dir21) + 1.0f) * 0.5f);
                right = Vector3.Cross(dir, camDir).normalized * Mathf.Lerp(1f, 0.5f, dot) * thickness;
                vertices.Add(points[i] - right); vertices.Add(points[i] + right);
                uvs.Add(new Vector2(0f, inv * i)); uvs.Add(new Vector2(1f, inv * i));
            }

            // add head
            dir = (points[count - 1] - points[count - 2]).normalized;
            right = Vector3.Cross(dir, camDir).normalized * 0.5f * thickness;
            vertices.Add(points[count - 1] - right); vertices.Add(points[count - 1] + right);
            uvs.Add(new Vector2(0f, 1f)); uvs.Add(new Vector2(1f, 1f));

            for(int i = 0, n = vertices.Count - 2; i < n; i += 2)
            {
                triangles.Add(i); triangles.Add(i + 1); triangles.Add(i + 2);
                triangles.Add(i + 3); triangles.Add(i + 2); triangles.Add(i + 1);
            }

            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetIndices(triangles.ToArray(), MeshTopology.Triangles, 0);

            return mesh;
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.white;
            points.ForEach(p =>
            {
                Gizmos.DrawSphere(p, 0.02f);
            });
        }

    }

}


