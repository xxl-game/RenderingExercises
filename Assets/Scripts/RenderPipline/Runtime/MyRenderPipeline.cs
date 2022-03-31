using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 自定义的渲染管线。
/// </summary>
public class MyRenderPipeline : RenderPipeline
{
    private MyRenderPipelineAsset asset;

    private ShaderTagId[] shaderTagIds;

    public MyRenderPipeline(MyRenderPipelineAsset asset)
    {
        this.asset = asset;
        var result = asset.shaderTagIds
            .Where(id => !string.IsNullOrEmpty(id))
            .Select(id => new ShaderTagId(id)).ToList();
        result.Insert(0, new ShaderTagId(""));
        shaderTagIds = result.ToArray();
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

        Debug.Log($"#Render#f:{Time.frameCount}");
        BeginFrameRendering(context, cameras);
        ClearingRenderTarget(context);
        
        for (var i = 0; i < cameras.Length; i++)
        {
            var camera = cameras[i];
            BeginCameraRendering(context, camera);
            var cullingResults = Culling(context, camera);
            
            context.SetupCameraProperties(camera);
            
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            
            var filteringSettings = FilteringSettings.defaultValue;
            var drawingSettings = TryCreateDrawingSettings(shaderTagIds, camera);
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

            if (camera.cameraType == CameraType.Game)
            {
                Debug.Log($"#Render#{i}|{cameras.Length}:{camera.name}");
            }
            else if (camera.cameraType == CameraType.SceneView)
            {
                if (asset.isDrawPostImageGizmo)
                {
                    context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
                }
            }

            if (asset.isDrawSkybox)
            {
                if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
                {
                    context.DrawSkybox(camera);
                }
            }
            
            context.Submit();
            EndCameraRendering(context, camera);
        }

        EndFrameRendering(context, cameras);
    }

    /// <summary>
    /// 清除渲染目标：移除上一帧绘制的几何体。
    /// </summary>
    public void ClearingRenderTarget(ScriptableRenderContext context)
    {
        if (asset.isClearRenderTarget)
        {
            var cmd = new CommandBuffer();
            cmd.ClearRenderTarget(asset.isClearDepth, asset.isClearColor, asset.backgroundColor, asset.depth);
            context.ExecuteCommandBuffer(cmd);
            cmd.Release();
        }
    }

    /// <summary>
    /// 剔除：过滤掉相机不可见的几何体。
    /// </summary>
    public CullingResults Culling(ScriptableRenderContext context, Camera camera)
    {
        if (!camera.TryGetCullingParameters(out var cullingParameters))
        {
            Debug.LogError("Error inVaild cam cull");
            return default;
        }

        var cullingResults = context.Cull(ref cullingParameters);
        return cullingResults;
    }

    /// <summary>
    /// 绘制：告诉GPU绘制什么几何体以及如何绘制。
    /// </summary>
    public void Drawing()
    {
    }

    /// <summary>
    /// Settings for ScriptableRenderContext.DrawRenderers
    /// </summary>
    /// <param name="shaderTagIds"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public DrawingSettings TryCreateDrawingSettings(ShaderTagId[] shaderTagIds, Camera camera)
    {
        if (shaderTagIds == null || shaderTagIds.Length == 0)
        {
            return new DrawingSettings(new ShaderTagId(""), new SortingSettings(camera));
        }

        DrawingSettings result = new DrawingSettings(shaderTagIds[0], new SortingSettings(camera));
        for (int i = 1; i < shaderTagIds.Length; ++i)
        {
            result.SetShaderPassName(i, shaderTagIds[i]);
        }
        return result;
    }
}