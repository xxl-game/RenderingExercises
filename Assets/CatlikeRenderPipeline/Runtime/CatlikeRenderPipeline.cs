using UnityEngine;
using UnityEngine.Rendering;

public class CatlikeRenderPipeline : RenderPipeline
{
    private CameraRenderer renderer = new CameraRenderer();

    public CatlikeRenderPipeline()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            renderer.Render(context, camera);
        }
    }
}