using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 根据材质和Shader名来命名物体。
/// matName_shaderName
/// </summary>
public class NameByShader : MonoBehaviour
{
    [Button(ButtonSizes.Gigantic)]
    [DisableInPrefabAssets]
    void ResetName()
    {
        var render = GetComponent<Renderer>();
        var mat = render.sharedMaterial;
        gameObject.name = $"{mat.name} ： {mat.shader.name}";
    }
}
