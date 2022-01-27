Shader "Hidden/MODev/GpuPA/TransparencyOverdrawOverlay"
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
		Blend One One
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			fixed _OverlayParam;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(_OverlayParam, 0, 0, 1);
			}
			ENDCG
		}
	}
}
