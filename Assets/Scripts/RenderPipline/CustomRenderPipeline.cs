using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPipeline : RenderPipeline
{
    private CustomRenderPipelineAsset asset;

    public CustomRenderPipeline(CustomRenderPipelineAsset asset)
    {
        this.asset = asset;
    }

    /// <summary>
    /// 写自定义渲染代码的地方。
    /// 每个渲染帧自动执行。
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cameras"></param>
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        Debug.Log("----------------");
        BeginFrameRendering(context, cameras);
        
        if (asset.isClearRenderTarget)
        {
            var cmd = new CommandBuffer();
            cmd.ClearRenderTarget(asset.isClearDepth, asset.isClearColor, asset.backgroundColor, asset.depth);
            context.ExecuteCommandBuffer(cmd);
            cmd.Release();
        }

        for (var i = 0; i < cameras.Length; i++)
        {
            var camera = cameras[i];
            Debug.Log($"{camera.cameraType}: {i}|{cameras.Length}|{camera.depth} : {camera.name}");
            camera.TryGetCullingParameters(out var cullingParameters);
            var cullingResults = context.Cull(ref cullingParameters);

            context.SetupCameraProperties(camera);

            var drawingSettings = new DrawingSettings(new ShaderTagId("CustomLight"), new SortingSettings(camera));
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

            if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
            {
                context.DrawSkybox(camera);
            }

            context.Submit();
        }
        
        EndFrameRendering(context, cameras);
    }
}