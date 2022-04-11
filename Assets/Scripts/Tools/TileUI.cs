using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TileUI : BaseMonoBehaviour
{
    public GameObject item;

    public GridLayoutGroup layout;

    public int gridW;

    public int gridH;

    public int w;

    public int h;

    public Button addW;
    
    public Button subW;

    public Button addH;
    
    public Button subH;
    
    public Text textInfo;
    
    public List<Material> materials;

    public List<Material> usedMaterials;

    public Color mainColor;

    public Color subColor;

    [PropertyOrder(1000)]
    public Sprite[] sprites;
    
    private void Awake()
    {
        materials = new List<Material>();
        usedMaterials = new List<Material>();
        Application.targetFrameRate = 30;
        if (null != addW)
        {
            addW.onClick.AddListener(AddW);
        }
        
        if (null != subW)
        {
            subW.onClick.AddListener(SubW);
        }

        if (null != addH)
        {
            addH.onClick.AddListener(AddH);   
        }

        if (null != subH)
        {
            subH.onClick.AddListener(SubH);
        }
        
        gridW = 2;
        gridH = 2;
    }

    private void Start()
    {
        Generate();
    }

    [Button]
    [HorizontalGroup]
    void AddH()
    {
        gridH += 2;
        Generate();            
    }

    [Button]
    [HorizontalGroup]
    void SubH()
    {
        if (gridH >= 2)
        {
            gridH -= 2;
        }
        Generate();            
    }

    [Button]
    [HorizontalGroup]
    void AddW()
    {
        gridW += 2;
        Generate();
    }

    [Button]
    [HorizontalGroup]
    void SubW()
    {
        if (gridW >= 2)
        {
            gridW -= 2;
            Generate();            
        }
    }

    [Button(ButtonSizes.Medium)]
    void Generate()
    {
        if (null != textInfo)
        {
            textInfo.text = $"{gridW*gridH}={gridW}x{gridH}";
        }
        
        Clear();
        if (null == item)
        {
            return;
        }

        var rectTransform = layout.GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        layout.cellSize = new Vector2(rect.width / gridW, rect.height / gridH);

        for (int i = 0; i < gridH; i++)
        {
            for (int j = 0; j < gridW; j++)
            {
                var grid = Instantiate(item, layout.transform);
                var img = grid.GetComponent<Image>();
                var flag = (i % 2 == 0) == (j % 2 != 0);
                img.color = flag ? mainColor : subColor;

                var mat = img.material;
                if (null != mat)
                {
                    img.material = new Material(img.material.shader);
                }

                img.sprite = sprites[Random.Range(0, sprites.Length)];

#if UNITY_EDITOR
                img.name = $"{i}_{i % 2} {j}_{j % 2} {mat.GetHashCode()}";
#endif
            }
        }
    }

    [Button(ButtonSizes.Medium)]
    void Clear()
    {
        layout.transform.DeleteAllChildren();
    }
}