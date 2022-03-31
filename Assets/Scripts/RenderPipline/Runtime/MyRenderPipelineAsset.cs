using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 自定义的渲染管线资产。
/// </summary>
[CreateAssetMenu(menuName = "Rendering/MyRenderPipelineAsset")]
public class MyRenderPipelineAsset : RenderPipelineAsset
{
    [ToggleGroup("isClearRenderTarget", ToggleGroupTitle = "清理渲染目标")]
    public bool isClearRenderTarget = true;

    [ToggleLeft]
    [ToggleGroup("isClearRenderTarget")]
    public bool isClearDepth = true;

    [ToggleLeft]
    [ToggleGroup("isClearRenderTarget")]
    public bool isClearColor = true;
    
    [ToggleGroup("isClearRenderTarget")]
    public Color backgroundColor;

    [ToggleGroup("isClearRenderTarget")]
    public float depth = 1f;

    [LabelText("是否绘制天空盒")]
    public bool isDrawSkybox = false;

    [Delayed]
    [PropertyOrder(100)]
    public string[] shaderTagIds;

    public bool isDrawPostImageGizmo;
    
    /// <summary>
    /// 第一次渲染之前会调用这个函数。
    /// 每当这个资产设置更改时，也会销毁之前的管线资产并重新调用这个函数来创建新的资产。
    /// </summary>
    /// <returns></returns>
    protected override RenderPipeline CreatePipeline()
    {
        Debug.Log($"#Asset#Create");
        Application.targetFrameRate = 10;
        return new MyRenderPipeline(this);
    }

    private void Reset()
    {
        isClearRenderTarget = true;
        isClearDepth = true;
        isClearColor = true;
        backgroundColor = Color.black;
        depth = 1f;
    }

    public bool IsValid()
    {
        if (shaderTagIds == null || shaderTagIds.Length == 0)
        {
            return false;
        }

        return true;
    }
}
