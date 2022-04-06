using UnityEngine;
using UnityEngine.Rendering;

public class CatlikeRenderPipeline : RenderPipeline
{
    private CatlikeCameraRenderer renderer = new CatlikeCameraRenderer();

    private bool useDynamicBatching;

    private bool useGPUInstancing;
    
    public CatlikeRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSrpBatcher)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSrpBatcher;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            renderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
        }
    }
}