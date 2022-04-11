using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CatlikeRenderPipeline")]
public class CatlikeRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField]
    public bool useDynamicBatching = true;

    [SerializeField]
    public bool useGPUInstancing;

    [SerializeField]
    public bool useSrpBatcher;

    [SerializeField]
    public bool isOverlay;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CatlikeRenderPipeline(this);
    }
}