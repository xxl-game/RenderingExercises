using Sirenix.OdinInspector;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
#if UNITY_EDITOR

    bool IsSameName()
    {
        return gameObject.name == GetType().Name;
    }

    [Button]
    [ShowIf("@!IsSameName()")]
    [PropertyOrder(1000)]
    void SetName()
    {
        gameObject.name = GetType().Name;
    }
    
#endif

}