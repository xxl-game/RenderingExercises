// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SimpleUnlitColor"
{
    Properties
    {
        _Color ("MainColor", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            float4 vert (appdata i) : SV_POSITION
            {
                return UnityObjectToClipPos(i.vertex);
            }

            fixed4 _Color;
            
            float4 frag () : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
