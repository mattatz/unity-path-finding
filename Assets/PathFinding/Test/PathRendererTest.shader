Shader "PathFinding/Test/PathRendererTest"
{

	Properties
	{
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
				float2 uv : TEXCOORD0;
				uint vid : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			StructuredBuffer<float3> _Vertices;
			int _VertexCount;
		
			v2f vert (appdata v, uint iid : SV_InstanceID)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				uint ioffset = iid * _VertexCount;
				o.vertex = UnityObjectToClipPos(float4(_Vertices[v.vid + ioffset].xyz, 1));
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(i.uv, 0, 1);
			}
			ENDCG
		}
	}
}
