using System.Collections.Generic;
using UnityEngine;

public static class TransformExtern
{
    public static void DeleteAllChildren(this Transform transform)
    {
        var itemList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            itemList.Add(transform.GetChild(i).gameObject);
        }

        for (var i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            if (null != item)
            {
                Object.DestroyImmediate(item.gameObject);
            }
        }
    }
}