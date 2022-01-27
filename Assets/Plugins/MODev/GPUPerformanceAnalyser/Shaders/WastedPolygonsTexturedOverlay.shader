Shader "Hidden/MODev/GpuPA/WastedPolygonsTexturedOverlay"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }

		Lighting Off
		Fog {Mode Off}
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Front

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
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texCol = tex2D(_MainTex, i.uv);
				texCol.a *= _OverlayParam;
				return texCol;
			}
			ENDCG
		}
	}
}
