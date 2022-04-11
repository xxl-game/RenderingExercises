using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CatlikeCameraRenderer
{
    partial void DrawUnsupportedShaders();

    partial void DrawGizmos();

    partial void PrepareForSceneWindow();

    partial void PrepareBuffer();

    partial void DrawOverlay();
    
#if UNITY_EDITOR

    private string SampleName { get; set; }

    private static Material errorMaterial;

    private static Material overlayMaterial;
    
    private static ShaderTagId[] legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };

    private static ShaderTagId overlayShaderTagId;

    partial void PrepareBuffer()
    {
        buffer.name = SampleName = camera.name;
    }

    partial void PrepareForSceneWindow()
    {
        if (camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }

    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void DrawOverlay()
    {
        if (null == overlayMaterial)
        {
            overlayMaterial = new Material(Shader.Find("Hidden/MODev/GpuPA/TransparencyOverdrawOverlay"));
        }

        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
        {
            overrideMaterial = overlayMaterial
        };
        for (var i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        drawingSettings.SetShaderPassName(legacyShaderTagIds.Length, unlitShaderTagId);
        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    partial void DrawUnsupportedShaders()
    {
        if (null == errorMaterial)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
        {
            overrideMaterial = errorMaterial
        };
        for (var i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }
#else
    
    const string SampleName = bufferName;
    
#endif

}