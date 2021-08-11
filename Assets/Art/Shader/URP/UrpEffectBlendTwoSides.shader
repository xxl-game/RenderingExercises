Shader "URPCustom/UrpEffectBlendTwoSides"
{
    Properties
    {
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MainTex("Main Tex", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_FrontFacesColor("Front Faces Color", Color) = (0,0.2313726,1,1)
		_BackFacesColor("Back Faces Color", Color) = (0.1098039,0.4235294,1,1)
		_Emission("Emission", Float) = 2
		[Toggle]_UseFresnel("Use Fresnel?", Float) = 1
		[Toggle]_SeparateFresnel("SeparateFresnel", Float) = 0
		_SeparateEmission("Separate Emission", Float) = 2
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_Fresnel("Fresnel", Float) = 1
		_FresnelEmission("Fresnel Emission", Float) = 1
		[Toggle]_UseCustomData("Use Custom Data?", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
    }
	
    SubShader
    {
        Tags 
    	{ 
    		"RenderPipeline" = "UniversalPipeline"
//    		"RenderType" = "TransparentCutout"
//    		"Queue" = "Transparent+0"
//    		"IsEmissive" = "true"
//    		"PreviewType" = "Plane"
        }
    	
    	Cull Off
		Blend SrcAlpha OneMinusSrcAlpha	

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };
            
			uniform float _SeparateFresnel;
			uniform float _UseFresnel;
			uniform float4 _FrontFacesColor;
			uniform float _Fresnel;
			uniform float _FresnelEmission;
			uniform float4 _FresnelColor;
			uniform float4 _BackFacesColor;
			uniform float _Emission;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _SpeedMainTexUVNoiseZW;
			uniform float _SeparateEmission;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;
			uniform sampler2D _Noise;
			uniform float4 _Noise_ST;
			uniform float _UseCustomData;
			uniform float _Cutoff = 0.5;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag () : SV_Target
            {
            	return _FresnelColor;
            }

            ENDHLSL
        }
    }
}
