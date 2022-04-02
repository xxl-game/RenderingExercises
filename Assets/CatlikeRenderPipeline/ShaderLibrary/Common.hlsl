#ifndef CUSTOM_COMMON_INCLUDED
#define CUSTOM_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "UnityInput.hlsl"


// float3 TransformObjectToWorld (float3 position0S)
// {
//     return mul(unity_ObjectToWorld, float4(position0S, 1.0)).xyz;
// }
//
// float4 TransformWorldToHClip(float3 positionWS)
// {
//     return mul(unity_MatrixVP, float4(positionWS, 1.0));
// }

#define UNITY_MATRIX_M unity_ObjectToWorld
#define UNITY_MATRIX_I_M unity_WorldToObject
#define UNITY_MATRIX_V unity_MatrixV
#define UNITY_MATRIX_VP unity_MatrixVP
#define UNITY_MATRIX_P glstate_matrix_projection

// GPU 实例化
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

// 空间转换
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

#endif