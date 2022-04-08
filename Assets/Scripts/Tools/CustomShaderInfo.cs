using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class CustomShaderInfo
{
    [HideLabel]
    [DisplayAsString]
    [HorizontalGroup("line")]
    [VerticalGroup("line/v2")]
    [GUIColor("GetColor")]
    public string name;

    [DisplayAsString]
    [HideLabel]
    [HideInInspector]
    public bool supported;

    [HideLabel]
    [DisplayAsString]
    [HideInInspector]
    public bool hasError;

    [HideLabel]
    [DisplayAsString]
    [HorizontalGroup("line")]
    [VerticalGroup("line/v2")]
    [GUIColor(.7f, .7f, .7f)]
    public string path;

    public bool IsHasPath
    {
        get { return !string.IsNullOrEmpty(path); }
    }

    [HideLabel]
    [ShowInInspector]
    [DisplayAsString]
    [HorizontalGroup("line", Width = 20f)]
    public int PropertyLength
    {
        get
        {
            return propertyInfos?.Length ?? 0;
        }
    }
    
    [HideLabel]
    [ShowInInspector]
    [PropertyOrder(-1)]
    [HorizontalGroup("line", Width = 5)]
    [VerticalGroup("line/v1")]
    private bool show;

    [Indent(1)]
    [ShowIf("IsShowProperty")]
    [ListDrawerSettings(Expanded = true, NumberOfItemsPerPage = 10, IsReadOnly = true)]
    public ShaderPropertyInfo[] propertyInfos;

    public bool IsHasProperty
    {
        get { return null != propertyInfos && propertyInfos.Length > 0; }
    }

    public bool IsShowProperty
    {
        get { return show && IsHasProperty; }
    }

    Color GetColor()
    {
        if (hasError)
        {
            return Color.red;
        }

        if (!supported)
        {
            return Color.magenta;
        }

        return Color.white;
    }

#if UNITY_EDITOR
    
    [Button]
    [EnableIf("IsHasPath")]
    [HorizontalGroup("line", Width = 50)]
    public void Select()
    {
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Shader>(path));
    }

    public CustomShaderInfo(UnityEditor.ShaderInfo shaderInfo, string path)
    {
        name = shaderInfo.name;
        supported = shaderInfo.supported;
        hasError = shaderInfo.hasErrors;
        this.path = path;
        var shader = Shader.Find(name);
        var propertyCount = ShaderUtil.GetPropertyCount(shader);
        if (propertyCount > 0)
        {
            propertyInfos = new ShaderPropertyInfo[propertyCount];
            for (int i = 0; i < propertyCount; i++)
            {
                var shaderPropertyInfo = new ShaderPropertyInfo();
                shaderPropertyInfo.name = ShaderUtil.GetPropertyName(shader, i);
                shaderPropertyInfo.t = ShaderUtil.GetPropertyType(shader, i);
                shaderPropertyInfo.des = ShaderUtil.GetPropertyDescription(shader, i);
                propertyInfos[i] = shaderPropertyInfo;
            }

            propertyInfos = propertyInfos.OrderBy(p => p.t).ToArray();
        }
    }
#endif

    [Serializable]
    public class ShaderPropertyInfo
    {
#if UNITY_EDITOR
        
        [HorizontalGroup(50f)]
        [HideLabel]
        [DisplayAsString]
        public UnityEditor.ShaderUtil.ShaderPropertyType t;
#endif

        [HorizontalGroup]
        [HideLabel]
        [DisplayAsString]
        public string name;

        [HorizontalGroup]
        [HideLabel]
        [DisplayAsString]
        public string des;
    }
}