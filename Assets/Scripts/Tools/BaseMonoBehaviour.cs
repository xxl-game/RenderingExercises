using Sirenix.OdinInspector;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
    [Button]
    [PropertyOrder(1000)]
    void SetName()
    {
        gameObject.name = GetType().Name;
    }
}