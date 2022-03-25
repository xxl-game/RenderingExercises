using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 自定义的渲染管线。
/// </summary>
public class CustomRenderPipeline : RenderPipeline
{
    private CustomRenderPipelineAsset asset;

    private ShaderTagId[] shaderTagIds;

    public CustomRenderPipeline(CustomRenderPipelineAsset asset)
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
        if (asset.shaderTagIds == null || asset.shaderTagIds.Length == 0)
        {
            return;
        }

        Debug.Log($"#Render#-------------------------------------- {Time.frameCount}");
        BeginFrameRendering(context, cameras);

        if (asset.isClearRenderTarget)
        {
            var cmd = new CommandBuffer();
            cmd.ClearRenderTarget(asset.isClearDepth, asset.isClearColor, asset.backgroundColor, asset.depth);
            context.ExecuteCommandBuffer(cmd);
            cmd.Release();
        }

        for (var i = 0; i < cameras.Length; i++)
        {
            var camera = cameras[i];
            var skyInfo = string.Empty;
            if (asset.isDrawSkybox)
            {
                if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
                {
                    skyInfo = $"sky:{RenderSettings.skybox.name}";
                    context.DrawSkybox(camera);
                }
            }

            if (!camera.TryGetCullingParameters(out var cullingParameters))
            {
                Debug.LogError("Error inVaild cam cull");
                continue;
            }

            var cullingResults = context.Cull(ref cullingParameters);

            context.SetupCameraProperties(camera);

            var drawingSettings = TryCreateDrawingSettings(shaderTagIds, camera);
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

            if (camera.cameraType == CameraType.Game)
            {
                Debug.Log($"#Render#{i}|{cameras.Length}_{camera.name} {skyInfo}");
            }

            context.Submit();
        }
        
        EndFrameRendering(context, cameras);
    }

    public DrawingSettings TryCreateDrawingSettings(ShaderTagId[] shaderTagIds, Camera camera)
    {
        if (shaderTagIds == null || shaderTagIds.Length == 0)
        {
            return new DrawingSettings(new ShaderTagId(""), new SortingSettings(camera));
        }

        DrawingSettings settings = new DrawingSettings(shaderTagIds[0], new SortingSettings(camera));
        for (int i = 1; i < shaderTagIds.Length; ++i)
        {
            settings.SetShaderPassName(i, shaderTagIds[i]);
        }
        return settings;
    }
}