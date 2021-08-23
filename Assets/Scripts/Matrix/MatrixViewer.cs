using Sirenix.OdinInspector;
using UnityEngine;

public class MatrixViewer : MonoBehaviour
{
    [OnValueChanged("OnMatrixChanged")]
    public Matrix4x4 matrix4X4;

    public Vector4 v;
    
    void OnMatrixChanged()
    {
        var t = transform;
        t.position = new Vector3(matrix4X4[0, 3], matrix4X4[1, 3], matrix4X4[2, 3]);
        t.rotation = matrix4X4.rotation;
        t.localScale = matrix4X4.lossyScale;
    }

    [Button]
    void ResetToTransform()
    {
        matrix4X4 = transform.localToWorldMatrix;
        var a = matrix4X4 * v;
    }
}
