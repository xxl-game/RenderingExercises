// This defines a simple unlit Shader object that is compatible with a custom Scriptable Render Pipeline.
// It applies a hardcoded color, and demonstrates the use of the LightMode Pass tag.
// It is not compatible with SRP Batcher.

Shader "CustomPipeline/SimpleUnlitColor"
{
    Properties 
    {
        _Color("Color", color) = (1, 1, 1, 1)
        _BaseMap("Base Map", 2D) = "white"
    }
    
    SubShader
    {
        Pass
        {
            // The value of the LightMode Pass tag must match the ShaderTagId in ScriptableRenderContext.DrawRenderers
            Tags
            {
                "LightMode" = "CustomLight"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;

            Texture2D(_BaseMap);
            sampler(sampler_BaseMap);
            
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 worldPos = mul(unity_ObjectToWorld, IN.positionOS);
                OUT.positionCS = mul(unity_MatrixVP, worldPos);

                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_TARGET
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}