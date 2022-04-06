using UnityEngine;
using UnityEngine.Rendering;

public class SimpleCameraRenderer
{
    private ScriptableRenderContext context;

    private Camera camera;
    
    private CullingResults cullingResults;

    private DrawingSettings drawingSettings;

    private FilteringSettings filteringSettings;

    public static SimpleCameraRenderer Instance;

    private CommandBuffer cmdBuffer;
    
    public SimpleCameraRenderer()
    {
        Instance = this;
        drawingSettings = new DrawingSettings
        {
            sortingSettings = new SortingSettings()
        };
        filteringSettings = new FilteringSettings();
        filteringSettings.renderQueueRange = RenderQueueRange.opaque;
        cmdBuffer = new CommandBuffer();
    }

    public void Render(ScriptableRenderContext context, Camera camera, SimpleRenderPipelineAsset asset)
    {
        this.context = context;
        this.camera = camera;

        context.SetupCameraProperties(camera);
        ClearRenderTarget();
        context.DrawSkybox(camera);
        context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
        context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        DrawRenderers(asset);
        
        context.Submit();
    }

    void ClearRenderTarget()
    {
        var flag = camera.clearFlags;
        var isClearDepth = flag <= CameraClearFlags.Depth;
        var isClearColor = flag == CameraClearFlags.Color;
        var backgroundColor = isClearColor ? camera.backgroundColor.linear : Color.clear;
        var depth = camera.depth;
        cmdBuffer.ClearRenderTarget(isClearDepth, isClearColor, backgroundColor, depth);
        ExecuteAndClearBuffer();
    }

    void ExecuteAndClearBuffer()
    {
        context.ExecuteCommandBuffer(cmdBuffer);
        cmdBuffer.Clear();
    }

    void DrawRenderers(SimpleRenderPipelineAsset asset)
    {
        camera.TryGetCullingParameters(out var cullingParameters);
        cullingResults = context.Cull(ref cullingParameters);
        
        for (var i = 0; i < asset.renderQueueRangeRecords.Length; i++)
        {
            var record = asset.renderQueueRangeRecords[i];
            
            var sortingSettings = new SortingSettings {criteria = record.sortingCriteria};
            drawingSettings.sortingSettings = sortingSettings;
            drawingSettings.enableDynamicBatching = true;
            drawingSettings.enableInstancing = true;
            
            filteringSettings.renderQueueRange = new RenderQueueRange(record.from, record.to);
            filteringSettings.sortingLayerRange = SortingLayerRange.all;
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        }
    }
}