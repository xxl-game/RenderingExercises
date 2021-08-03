using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectWatcher : MonoBehaviour
{
    public enum RenderPipelineType
    {
        None,
        Urp
    }

    public enum CameraViewType
    {
        Only2D,
        Only3D,
        Both,
    }
    
    [EnumToggleButtons]
    [PropertyOrder(100)]
    [OnValueChanged("ChangePipeline")]
    public RenderPipelineType renderPipelineType;

    [EnumToggleButtons]
    [PropertyOrder(100)]
    [OnValueChanged("ChangeCameraViewType")]
    public CameraViewType cameraViewType;

    [PropertyOrder(100)]
    [OnValueChanged("OnColorChanged")]
    public Color backGroundColor;
    
    public RenderPipelineAsset renderPipelineAsset;
    
    public Camera camera2D;
    
    public Camera camera3D;
    
    void ChangePipeline()
    {
        switch (renderPipelineType)
        {
            case RenderPipelineType.None:
                GraphicsSettings.renderPipelineAsset = null;
                break;
            case RenderPipelineType.Urp:
                GraphicsSettings.renderPipelineAsset = renderPipelineAsset;
                break;
        }
    }

    void ChangeCameraViewType()
    {
        if (null == camera2D || null == camera3D)
        {
            return;
        }
        
        switch (cameraViewType)
        {
            case CameraViewType.Only2D:
                camera2D.gameObject.SetActive(true);
                camera3D.gameObject.SetActive(false);
                camera2D.rect = new Rect(0f, 0f, 1f, 1f);
                break;
            case CameraViewType.Only3D:
                camera2D.gameObject.SetActive(false);
                camera3D.gameObject.SetActive(true);
                camera3D.rect = new Rect(0f, 0f, 1f, 1f);
                break;
            case CameraViewType.Both:
                camera2D.gameObject.SetActive(true);
                camera3D.gameObject.SetActive(true);
                camera2D.rect = new Rect(0f, 0f, .5f, 1f);
                camera3D.rect = new Rect(.5f, 0f, .5f, 1f);
                break;
        }
    }

    void OnColorChanged()
    {
        camera2D.clearFlags = CameraClearFlags.SolidColor;
        camera2D.backgroundColor = backGroundColor;
        camera3D.clearFlags = CameraClearFlags.SolidColor;
        camera3D.backgroundColor = backGroundColor;
    }

    private void Reset()
    {
        gameObject.name = nameof(EffectWatcher);
        var transform2D = GameObject.Find("Camera2D");
        if (null != transform2D)
        {
            camera2D = transform2D.GetComponent<Camera>();
        }
        var transform3D = GameObject.Find("Camera3D");
        if (null != transform3D)
        {
            camera2D = transform3D.GetComponent<Camera>();
        }
    }
}
