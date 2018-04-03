using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Rendering;

namespace PathFinding
{

    public class NodeRenderer : MonoBehaviour {

        [SerializeField] protected Mesh mesh;
        [SerializeField] protected Material material;

        protected ComputeBuffer positionBuffer, argsBuffer;
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

        #region MonoBehaviour functions

        protected void Start () {
        }
        
        protected void Update () {
            if (argsBuffer == null) return;

            material.SetBuffer("_Position", positionBuffer);
            material.SetMatrix("_WorldToLocal", transform.worldToLocalMatrix);
            material.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
            Graphics.DrawMeshInstancedIndirect(mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 1000f), argsBuffer, 0, null);
        }

        protected void OnDestroy()
        {
            if(argsBuffer != null)
            {
                positionBuffer.Release();
                argsBuffer.Release();
            }
        }

        #endregion

        public void Setup(Graph graph)
        {
            args[0] = mesh.GetIndexCount(0);
            args[1] = (uint)graph.Nodes.Count;
            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            argsBuffer.SetData(args);

            positionBuffer = new ComputeBuffer(graph.Nodes.Count, Marshal.SizeOf(typeof(Vector3)));
            positionBuffer.SetData(graph.Nodes.Select(node => node.Position).ToArray());
        }
        
    }

}


