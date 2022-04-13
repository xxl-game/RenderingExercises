using UnityEngine;
using UnityEngine.Rendering;

public class CatlikeRenderPipeline : RenderPipeline
{
    private CatlikeCameraRenderer renderer = new CatlikeCameraRenderer();

    private CatlikeRenderPipelineAsset asset;
    
    public CatlikeRenderPipeline(CatlikeRenderPipelineAsset asset)
    {
        this.asset = asset;
        GraphicsSettings.useScriptableRenderPipelineBatching = asset.useSrpBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            renderer.Render(context, camera, asset);
        }
    }
}