using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField]
    private Color baseColor = Color.white;

    private static MaterialPropertyBlock block;

    private void OnValidate()
    {
        if (null == block)
        {
            block = new MaterialPropertyBlock();
        }
        
        block.SetColor(baseColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
