using Sirenix.OdinInspector;
using UnityEngine;

public class NameByShader : MonoBehaviour
{
    [Button(ButtonSizes.Gigantic)]
    [DisableInPrefabAssets]
    void ResetName()
    {
        gameObject.name = GetComponent<Renderer>().sharedMaterial.shader.name;
    }
}
