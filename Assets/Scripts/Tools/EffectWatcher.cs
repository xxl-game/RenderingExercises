using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 切换渲染管线和相机
/// </summary>
public class EffectWatcher : MonoBehaviour
{
    public Color backGroundColor;

    public RenderPipelineAsset[] pipelineAssets;

    public Camera[] cameras;

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
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = backGroundColor;
        }
    }

    private void Reset()
    {
        gameObject.name = nameof(EffectWatcher);
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