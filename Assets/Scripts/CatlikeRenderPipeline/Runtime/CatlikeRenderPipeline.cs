using UnityEngine;
using UnityEngine.Rendering;

public class CatlikeRenderPipeline : RenderPipeline
{
    private CameraRenderer renderer = new CameraRenderer();
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            renderer.Render(context, camera);
        }
    }
}