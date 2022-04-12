using UnityEngine;
using UnityEngine.Rendering;

public partial class CatlikeCameraRenderer
{
    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");    
    
    private static ShaderTagId litShaderTagId = new ShaderTagId("CatlikeCustomLit");    
    
    private ScriptableRenderContext context;

    private Camera camera;

    private const string bufferName = "Render Camera";

    private CommandBuffer buffer = new CommandBuffer {name = bufferName};

    private CullingResults cullingResults;

    private bool useDynamicBatching;
    private bool useGPUInstancing;

    private static Material overlayMaterial;
    
    private CatlikeRenderPipelineAsset asset;
    
    public void Render(ScriptableRenderContext context, Camera camera, CatlikeRenderPipelineAsset asset)
    {
        this.context = context;
        this.camera = camera;
        this.asset = asset;

        PrepareBuffer();
        PrepareForSceneWindow();
        if (!Cull())
        {
            return;
        }
        
        Setup();
        if (null != asset && asset.isOverlay)
        {
            DrawOverlay();            
        }
        else
        {
            DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
            DrawUnsupportedShaders();            
        }

        DrawGizmos();
        Submit();
    }

    void DrawOverlay()
    {
        if (null == overlayMaterial)
        {
            overlayMaterial = new Material(Shader.Find("Hidden/MODev/GpuPA/TransparencyOverdrawOverlay"));
        }

        // Debug.Log(1);
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
        {
            overrideMaterial = overlayMaterial
        };
        // Debug.Log(2);
        for (var i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        // Debug.Log(3);
        drawingSettings.SetShaderPassName(legacyShaderTagIds.Length, unlitShaderTagId);
        // Debug.Log(4);
        var filteringSettings = FilteringSettings.defaultValue;
        // Debug.Log(5);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        // Debug.Log("Draw overlay");
    }
    
    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        // 从前到后绘制不透明物体
        var sortingSettings = new SortingSettings(camera) {criteria = SortingCriteria.CommonOpaque};
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        drawingSettings.enableDynamicBatching = useDynamicBatching;
        drawingSettings.enableInstancing = useGPUInstancing;
        drawingSettings.SetShaderPassName(1, litShaderTagId);
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
        // 设置相机属性
        context.SetupCameraProperties(camera);
        
        // 清理渲染目标。
        var flags = camera.clearFlags;
        var clearDepth = flags <= CameraClearFlags.Depth;
        var clearColor = flags == CameraClearFlags.Color;
        var backGroundColor = clearColor ? camera.backgroundColor.linear : Color.clear;
        buffer.ClearRenderTarget(clearDepth, clearColor, backGroundColor);
        buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }
    
    /// <summary>
    /// 提交命令给GPU
    /// </summary>
    void Submit()
    {
        buffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    /// <summary>
    /// 执行并清理命令缓存
    /// </summary>
    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    /// <summary>
    /// 裁切
    /// </summary>
    /// <returns></returns>
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