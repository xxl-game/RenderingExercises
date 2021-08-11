Shader "Custom/WorldSpaceNormals"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 pos : POSITION;
                float3 worldNormal : TEXCOORD0;
            };
            
            v2f vert(appdata i)
            {
                v2f O;
                O.pos = UnityObjectToClipPos(i.vertex);
                O.worldNormal = UnityObjectToWorldNormal(i.normal);
                return O;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 o = 0;
                o.rgb = i.worldNormal * 0.5 + 0.5;
                return o;
            }            
            ENDCG
        }
    }
}
