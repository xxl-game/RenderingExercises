using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class RenderQueueRangeRecord
{
    [Min(0f)]
    [HideLabel]
    [HorizontalGroup]
    public int from;
        
    [MaxValue(5000)]
    [HideLabel]
    [HorizontalGroup]
    public int to;

    [HideLabel]
    public SortingCriteria sortingCriteria;
    
}

[Serializable]
public class ShaderRecord
{
    [HideLabel]
    [HorizontalGroup]
    public string lightModeName;

    [HideLabel]
    [PropertyOrder(-1)]
    [HorizontalGroup(20)]
    public bool isEnable = false;
}

/// <summary>
/// 自定义的渲染管线资产。
/// </summary>
[CreateAssetMenu(menuName = "Rendering/MyRenderPipelineAsset")]
public class SimpleRenderPipelineAsset : RenderPipelineAsset
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

    public bool isDrawPostImageGizmo;

    [Delayed]
    [PropertyOrder(100)]
    [ListDrawerSettings(Expanded = true, DraggableItems = false)]
    [OnValueChanged("OnShaderTagIdsChanged", true)]
    public ShaderRecord[] shaderTagIds;

    [PropertyOrder(101)]
    [LabelText("From->To (0,2500,5000)")]
    [ListDrawerSettings(Expanded = true)]
    public RenderQueueRangeRecord[] renderQueueRangeRecords;

    public bool enableSrpBatching = true;
    
    /// <summary>
    /// 第一次渲染之前会调用这个函数。
    /// 每当这个资产设置更改时，也会销毁之前的管线资产并重新调用这个函数来创建新的资产。
    /// </summary>
    /// <returns></returns>
    protected override RenderPipeline CreatePipeline()
    {
        Debug.Log($"#Asset#Create");
        Application.targetFrameRate = 10;
        return new SimpleRenderPipeline(this);
    }

    void OnShaderTagIdsChanged()
    {
        Debug.Log("Changed");
        shaderTagIds = shaderTagIds
            .OrderBy(s => !s.isEnable).ToArray();
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
