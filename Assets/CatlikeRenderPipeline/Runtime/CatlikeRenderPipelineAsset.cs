using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CatlikeRenderPipeline")]
public class CatlikeRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField]
    private bool useDynamicBatching = true;

    [SerializeField]
    private bool useGPUInstancing;

    [SerializeField]
    private bool useSrpBatcher;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CatlikeRenderPipeline(useDynamicBatching, useGPUInstancing, useSrpBatcher);
    }
}