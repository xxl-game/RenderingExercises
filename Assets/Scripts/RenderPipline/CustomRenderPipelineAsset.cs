﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CustomRenderPipelineAsset")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [BoxGroup("ClearRenderTarget")][ToggleLeft]
    public bool isClearRenderTarget = true;

    [BoxGroup("ClearRenderTarget")][ToggleLeft]
    public bool isClearDepth = true;

    [BoxGroup("ClearRenderTarget")][ToggleLeft]
    public bool isClearColor = true;
    
    [BoxGroup("ClearRenderTarget")]
    public Color backgroundColor;

    [BoxGroup("ClearRenderTarget")]
    public float depth = 1f;

    [BoxGroup("Culling")][ToggleLeft]
    public bool isCulling;
    
    /// <summary>
    /// 第一次渲染之前会调用这个函数。
    /// 每当这个资产设置更改时，也会销毁之前的管线资产并重新调用这个函数来创建新的资产。
    /// </summary>
    /// <returns></returns>
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(this);
    }

    private void Reset()
    {
        isClearRenderTarget = true;
        isClearDepth = true;
        isClearColor = true;
        backgroundColor = Color.black;
        depth = 1f;
        isCulling = true;
    }
}