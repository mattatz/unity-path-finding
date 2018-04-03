Shader "PathFinding/NodeRenderer"
{

	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Size ("Size", Float) = 0.25
		_SrcBlend ("SrcBlend", Int) = 5.0 // SrcAlpha
		_DstBlend ("DstBlend", Int) = 10.0 // OneMinusSrcAlpha
		_ZWrite ("ZWrite", Int) = 1.0 // On
		_ZTest ("ZTest", Int) = 4.0 // LEqual
		_Cull ("Cull", Int) = 0.0 // Off
		_ZBias ("ZBias", Float) = 0.0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			Cull [_Cull]
			Offset [_ZBias], [_ZBias]

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup

			#include "UnityCG.cginc"

			float4x4 _WorldToLocal, _LocalToWorld;

			StructuredBuffer<float3> _Position;
			float _Size;

			float4 _Color;

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

			void setup()
			{
				unity_ObjectToWorld = _LocalToWorld;
				unity_WorldToObject = _WorldToLocal;
			}

			v2f vert (appdata v, uint iid : SV_InstanceID)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 translation = _Position[iid].xyz;
				v.vertex.xyz *= _Size;
				v.vertex.xyz += translation;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}

			ENDCG
		}
	}
}
