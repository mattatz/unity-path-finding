using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace PathFinding.Test
{

    public class PathRendererTest : MonoBehaviour {

        [SerializeField] protected ComputeShader compute;
        [SerializeField] protected Material material;
        [SerializeField] protected int count = 20;
        [SerializeField] protected float thickness = 0.25f;

        protected Mesh mesh;
        protected ComputeBuffer segmentBuffer, vertexBuffer;

        void Start () {
            mesh = PathRenderer.Build(count);

            Random.InitState(0);

            segmentBuffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Segment_t)));
            var segments = new Segment_t[count];
            for(int i = 0; i < count; i++) {
                segments[i].position = new Vector3(Random.value - 0.5f, i, Random.value - 0.5f);
            }
            segmentBuffer.SetData(segments);

            vertexBuffer = new ComputeBuffer(count * 2, Marshal.SizeOf(typeof(Vector3)));
            vertexBuffer.SetData(new Vector3[vertexBuffer.count]);
        }
        
        void Update () {
            ViewAlign();
            material.SetBuffer("_Vertices", vertexBuffer);
            material.SetInt("_VertexCount", count * 2);
            Graphics.DrawMeshInstanced(mesh, 0, material, new Matrix4x4[1] { transform.localToWorldMatrix }, 1);
        }

        protected void ViewAlign()
        {
            var kernel = compute.FindKernel("ViewAlign");
            compute.SetBuffer(kernel, "_Segments", segmentBuffer);
            compute.SetBuffer(kernel, "_Vertices", vertexBuffer);
            compute.SetInt("_SegmentsCount", count);
            compute.SetInt("_InstancesCount", 1);
            compute.SetFloat("_Thickness", thickness);

            var localCamDir = transform.InverseTransformDirection(Camera.main.transform.forward);
            compute.SetVector("_LocalCameraDirection", localCamDir);

            uint tx, ty, tz;
            compute.GetKernelThreadGroupSizes(kernel, out tx, out ty, out tz);

            compute.Dispatch(kernel, count / (int)tx + 1, (int)ty, (int)tz);
        }

        protected void OnDestroy()
        {
            segmentBuffer.Release();
            vertexBuffer.Release();
        }

    }

}


