using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 自定义的渲染管线。
/// </summary>
public class SimpleRenderPipeline : RenderPipeline
{
    private SimpleRenderPipelineAsset asset;

    private ShaderTagId[] shaderTagIds;

    private SimpleCameraRenderer simpleCameraRenderer = new SimpleCameraRenderer();
    
    public SimpleRenderPipeline(SimpleRenderPipelineAsset asset)
    {
        this.asset = asset;
        shaderTagIds = asset.shaderTagIds
            .Where(id => id.isEnable)
            .Select(id => new ShaderTagId(id.lightModeName))
            .ToArray();
        GraphicsSettings.useScriptableRenderPipelineBatching = asset.enableSrpBatching;
    }

    /// <summary>
    /// 写自定义渲染代码的地方。
    /// 每个渲染帧自动执行。
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cameras"></param>
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        if (!asset.IsValid())
        {
            return;
        }

        BeginFrameRendering(context, cameras);

        for (var i = 0; i < cameras.Length; i++)
        {
            var camera = cameras[i];
            BeginCameraRendering(context, camera);
            simpleCameraRenderer.Render(context, camera, asset);
            EndCameraRendering(context, camera);
        }

        EndFrameRendering(context, cameras);
    }
}