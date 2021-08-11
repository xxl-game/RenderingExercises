using Sirenix.OdinInspector;
using UnityEngine;

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
