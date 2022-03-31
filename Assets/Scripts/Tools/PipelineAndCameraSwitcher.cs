using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 切换渲染管线和相机
/// </summary>
public class PipelineAndCameraSwitcher : MonoBehaviour
{
    public Color backGroundColor;

    public RenderPipelineAsset[] pipelineAssets;

    public Camera[] cameras;

    [Space]
    [PropertyOrder(100)]
    [LabelText("所有Shader")]
    [ListDrawerSettings(ShowPaging = true, NumberOfItemsPerPage = 10, IsReadOnly = true, OnTitleBarGUI = "ShadersTitle")]
    [Searchable]
    public List<CustomShaderInfo> shaderInfos;

    void UpdateShaders()
    {
        var dict = AssetDatabase.FindAssets("t:Shader")
            .Select(AssetDatabase.GUIDToAssetPath)
            .OrderBy(s=>s)
            .ToArray()
            .ToDictionary(s=>AssetDatabase.LoadAssetAtPath<Shader>(s).name, s=>s);

        shaderInfos = ShaderUtil.GetAllShaderInfo()
            .Select(s =>
            {
                dict.TryGetValue(s.name, out var result);
                return new CustomShaderInfo(s, result);
            })
            .OrderBy(s=>!s.IsHasPath)
            .ThenBy(s=>s.path)
            .ThenBy(s=>s.name)
            .ToList();

        var last = dict.Values.ToList().Where(s => shaderInfos.Find(info=>info.name == s) != null).ToList();
        if (last.Count > 0)
        {
            Debug.LogError($"last: {last.Count}");
        }
    }

    void BeginPipelines(int index)
    {
        GUILayout.BeginHorizontal();
        var asset = pipelineAssets[index];
        var color = GUI.backgroundColor;
        if (GraphicsSettings.renderPipelineAsset == asset)
        {
            GUI.backgroundColor = Color.yellow;
        }

        if (GUILayout.Button("√", GUILayout.Width(40f)))
        {
            GraphicsSettings.renderPipelineAsset = asset;
        }

        GUI.backgroundColor = color;
    }

    void EndPipelines(int index)
    {
        GUILayout.EndHorizontal();
    }

    void BeginCameras(int index)
    {
        GUILayout.BeginHorizontal();
        var cam = cameras[index];
        if (null == cam)
        {
            return;
        }
        var color = GUI.backgroundColor;
        if (cam.gameObject.activeSelf)
        {
            GUI.backgroundColor = Color.yellow;
        }

        if (GUILayout.Button("√", GUILayout.Width(40)))
        {
            cam.gameObject.SetActive(!cam.gameObject.activeSelf);
            var activeCams = cameras.Where(c => c.gameObject.activeSelf).ToArray();
            var width = 1f / activeCams.Length;
            for (var i = 0; i < activeCams.Length; i++)
            {
                var iCam = activeCams[i];
                iCam.rect = new Rect(width * i, 0f, width, 1f);
            }
        }

        GUI.backgroundColor = color;
    }

    void EndCameras(int index)
    {
        GUILayout.EndHorizontal();
    }

    void OnColorChanged()
    {
        for (var i = 0; i < cameras.Length; i++)
        {
            var cam = cameras[i];
            if (null != cam)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = backGroundColor;                
            }
        }
    }

    void ShadersTitle()
    {
        if (GUILayout.Button("更新"))
        {
            UpdateShaders();
        }
    }

    private void Reset()
    {
        gameObject.name = nameof(PipelineAndCameraSwitcher);
        cameras = new Camera[2];

        var transform2D = GameObject.Find("Camera2D");
        if (null != transform2D)
        {
            cameras[1] = transform2D.GetComponent<Camera>();
        }

        var transform3D = GameObject.Find("Camera3D");
        if (null != transform3D)
        {
            cameras[2] = transform3D.GetComponent<Camera>();
        }
    }
}