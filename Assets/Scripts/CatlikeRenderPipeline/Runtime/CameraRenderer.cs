using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");    
    
    private ScriptableRenderContext context;

    private Camera camera;

    private const string bufferName = "Render Camera";

    private CommandBuffer buffer = new CommandBuffer {name = bufferName};

    private CullingResults cullingResults;
    
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();
        if (!Cull())
        {
            return;
        }
        
        Setup();
        DrawVisibleGeometry();
        DrawUnsupportedShaders();
        DrawGizmos();
        Submit();
    }
    
    void DrawVisibleGeometry()
    {
        // 从前到后绘制不透明物体
        var sortingSettings = new SortingSettings(camera) {criteria = SortingCriteria.CommonOpaque};
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filterSettings = new FilteringSettings(RenderQueueRange.opaque);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filterSettings);
        
        // 绘制天空盒
        context.DrawSkybox(camera);

        // 绘制透明物体
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filterSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filterSettings);
    }
    
    void Setup()
    {
        context.SetupCameraProperties(camera);
        var flags = camera.clearFlags;
        var clearDepth = flags <= CameraClearFlags.Depth;
        var clearColor = flags == CameraClearFlags.Color;
        var backGroundColor = clearColor ? camera.backgroundColor.linear : Color.clear;
        buffer.ClearRenderTarget(clearDepth, clearColor, backGroundColor);
        buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }
    
    void Submit()
    {
        buffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);   
            return true;
        }

        return false;
    }
}