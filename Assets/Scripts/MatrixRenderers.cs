using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MatrixRenderers : MonoBehaviour
{
    [Serializable]
    public class MaterialDrawer
    {
        [PropertyOrder(1)]
        [HideLabel]
        [HorizontalGroup]
        public Material material;

        [PropertyOrder(0)]
        [HideLabel]
        [HorizontalGroup(Width = 20f)]
        public bool enable = true;

        public override string ToString()
        {
            return $"{material.name} : {material.shader.name}";
        }
    }

    [Serializable]
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
        public List<MaterialDrawer> materialDrawers;

        public bool IsEmpty()
        {
            if (null == materialDrawers || materialDrawers.Count == 0)
            {
                return true;
            }

            for (var i = 0; i < materialDrawers.Count; i++)
            {
                var drawer = materialDrawers[i];
                if (drawer.enable && null != drawer.material)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public GameObject prefab;

    public Transform prefabRoot;
    
    public Line[] lines;

    private bool enable;

    public Vector3 euler;
    
    public Vector3 interval1;
    
    public Vector3 interval2;

    public Vector2 rootPos;

    public Vector3 rootEuler;
    
    public float rootScale;

    [HideInInspector]
    public List<Transform> renderObjects;

    public List<Transform> RenderObjects
    {
        get
        {
            if (null == renderObjects)
            {
                renderObjects = new List<Transform>();
            }
            return renderObjects;
        }
    }
    
    void OnScaleChanged()
    {
        transform.localScale = new Vector3(rootScale, rootScale, rootScale);
    }

    void OnEulerChanged()
    {
        for (var i = 0; i < RenderObjects.Count; i++)
        {
            var renderObject = RenderObjects[i];
            renderObject.localEulerAngles = euler;
        }
    }

    void OnRootPosChanged()
    {
        transform.localPosition = rootPos;
    }

    void OnRootEulerChanged()
    {
        transform.localEulerAngles = rootEuler;
    }

    void OnIntervalChanged()
    {
        
    }

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

    void ReCreate()
    {
        RenderObjects.Clear();
        ClearChildren(prefabRoot);

        if (null == prefab || null == lines || null == prefabRoot)
        {
            return;
        }

        var linesNew = lines.Where(line => !line.IsEmpty()).ToArray();
        for (var i = 0; i < linesNew.Length; i++)
        {
            var materialDrawers = linesNew[i].materialDrawers.Where(drawer => drawer.enable).ToArray();
            for (var j = 0; j < materialDrawers.Length; j++)
            {
                var materialDrawer = materialDrawers[j];
                var pos = i * interval1 + j * interval2;
                var t = Instantiate(prefab, prefabRoot).transform;
                t.localPosition = pos;
                t.localEulerAngles = euler;
                RenderObjects.Add(t);
                t.name = $"{i}-{j}-{materialDrawer}";
                var renderers = t.GetComponentsInChildren<Renderer>();
                for (var k = 0; k < renderers.Length; k++)
                {
                    var r = renderers[k];
                    r.material = materialDrawer.material;
                }
            }
        }
    }

    [Button("GetByShader")]
    void GetFromSelection()
    {
        RenderObjects.Clear();
        ClearChildren(prefabRoot);
        var materials = Selection.GetFiltered<Material>(SelectionMode.Assets | SelectionMode.DeepAssets).OrderBy(mat=>mat.name);
        var matGroups = materials.GroupBy(mat => mat.shader.name).ToList();
        lines = new Line[matGroups.Count];
        for (var i = 0; i < matGroups.Count; i++)
        {
            var matGroup = matGroups[i];
            var line = new Line();
            lines[i] = line;
            line.enable = true;
            line.name = matGroup.Key;
            line.materialDrawers = new List<MaterialDrawer>();
            foreach (var material in matGroup)
            {
                var matDrawer = new MaterialDrawer();
                matDrawer.enable = true;
                matDrawer.material = material;
                line.materialDrawers.Add(matDrawer);
                Debug.Log($"k: {matGroup.Key} , {material.name}");
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




























