Shader "Hidden/MODev/GpuPA/TransparencyTexturedOverlay"
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
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			fixed _OverlayParam;
			sampler2D _MainTex;
			float4 _MainTex_ST;

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col = lerp(col, fixed4(1, 0, 0, 1), 1.0 - col.a);
				col.a *= _OverlayParam;
				return col;
			}
			ENDCG
		}
	}
}
