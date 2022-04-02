using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomObjectCreator : MonoBehaviour
{
    public GameObject item;

    private List<GameObject> itemList;

    public int num = 100;

    public float xSize = 5;

    public float ySize = 5;

    public float zSize = 5;

    public List<Material> mats;
    
    [Button]
    void Create()
    {
        if (null == itemList)
        {
            itemList = new List<GameObject>();
        }
        for (int i = 0; i < num; i++)
        {
            var itemNew = Instantiate(item, transform);
            var x = Random.Range(-xSize, xSize);
            var y = Random.Range(-ySize, ySize);
            var z = Random.Range(-zSize, zSize);
            itemNew.transform.position = new Vector3(x, y, z);
            itemNew.GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Count)];
            itemList.Add(itemNew);
        }
    }

    [Button]
    void Clear()
    {
        if (null == itemList)
        {
            return;
        }
    
        itemList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            itemList.Add(transform.GetChild(i).gameObject);            
        }
        for (var i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            if (null != item)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
