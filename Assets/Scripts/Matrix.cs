using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Matrix : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        [HideLabel]
        [EnableIf("enable")]
        public string name;

        [HideInInspector]
        public bool enable;
        
        [LabelText(" ")]
        [EnableIf("enable")]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public Material[] materials;
    }

    [FormerlySerializedAs("prefabs")]
    [ShowIf("$enable")]
    [HideLabel]
    [AssetsOnly]
    public GameObject prefab;

    public Transform prefabRoot;
    
    [LabelText(" ")]
    [ListDrawerSettings]
    public Line[] lines;

    private bool enable;

    public Vector3 euler;
    public Vector3 interval1;
    public Vector3 interval2;

    [OnValueChanged("OnScaleChanged")]
    public float scale;

    void OnScaleChanged()
    {
        if (null != prefabRoot)
        {
            prefabRoot.localScale = new Vector3(scale, scale, scale);
        }
    }

    [Button]
    public void ToggleEnable()
    {
        enable = !enable;
        if (null != lines)
        {
            foreach (var line in lines)
            {
                line.enable = enable;
            }
        }
    }

    [Button(ButtonSizes.Medium)]
    void ReCreate()
    {
        ClearChildren(prefabRoot);
        
        if (null == prefab || null == lines || null == prefabRoot)
        {
            return;
        }

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.materials.Length; j++)
            {
                var material = line.materials[j];
                var t = Instantiate(prefab, i * interval1 + j * interval2, Quaternion.Euler(euler), prefabRoot).transform;
                t.name = $"{i}-{j}-{material.name}";
                var renderers = t.GetComponentsInChildren<Renderer>();
                for (var k = 0; k < renderers.Length; k++)
                {
                    var r = renderers[k];
                    r.material = material;
                }
            }
        }
        
    }
    
    public void ClearChildren(Transform t)
    {
        if (null == t)
        {
            return;
        }
        var i = 0;
        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[t.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in t)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}




























