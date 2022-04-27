Shader "Catlike RP/WastAlpha"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Lighting Off
		Fog {Mode Off}
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		

		Pass
		{
			
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "../ShaderLibrary/Common.hlsl"

			float4 _OverlayParam;
			float4 _OverlayParam2;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				float3 a = TransformObjectToWorld(v.vertex);
				o.vertex = TransformWorldToHClip(a);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float4 frag(v2f i) : SV_Target
			{
				float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);

				return float4(1, 1, 1, 1);
			}
			ENDHLSL
		}
	}
}
