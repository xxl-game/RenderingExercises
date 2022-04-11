using System.Collections;
using System.Collections.Generic;
using CodeStage.AdvancedFPSCounter;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TileUI : BaseMonoBehaviour
{
    [FoldoutGroup("引用")]
    public GameObject item;

    [FoldoutGroup("引用")]
    public Canvas layout;

    [BoxGroup("Opera")]
    public int gridW;

    [BoxGroup("Opera")]
    public int gridH;

    [BoxGroup("Opera")]
    public int w;

    [BoxGroup("Opera")]
    public int h;

    public bool isDelay = false;
    
    [HideInInspector]
    public List<Material> materials;

    [HideInInspector]
    public List<Material> usedMaterials;

    [HorizontalGroup("引用/Color")]
    [HideLabel]
    [PropertyOrder(-1)]
    public Color mainColor;

    [HorizontalGroup("引用/Color")]
    [HideLabel]
    [PropertyOrder(-1)]
    public Color subColor;

    [PropertyOrder(1000)]
    public Sprite[] sprites;

    [BoxGroup("Opera")]
    public bool isSetSprite = true;

    [BoxGroup("Opera")]
    public bool isFillScreen = true;

    [FoldoutGroup("引用")]
    public Button addW;

    [FoldoutGroup("引用")]
    public Button subW;

    [FoldoutGroup("引用")]
    public Button addH;

    [FoldoutGroup("引用")]
    public Button subH;
    
    [FoldoutGroup("引用")]
    public Button addWS;

    [FoldoutGroup("引用")]
    public Button subWS;

    [FoldoutGroup("引用")]
    public Button addHS;

    [FoldoutGroup("引用")]
    public Button subHS;

    [FoldoutGroup("引用")]
    public Text textInfoGridSize;
    
    [FoldoutGroup("引用")]
    public Text textInfoRealSize;

    [FoldoutGroup("引用")]
    public Text textTotal;

    [FoldoutGroup("引用")]
    public Canvas operaCanvas;

    private AFPSCounter afpsCounter;

    [FoldoutGroup("引用")]
    public CatlikeRenderPipelineAsset asset;

    [MinValue(1)]
    [BoxGroup("Opera")]
    public int layer = 1;

    [Range(0.01f, 0.2f)]
    public float minValue = 0.1f;
    
    [DisplayAsString]
    [GUIColor(0f, 1f, 0f, 1f)]
    public string overdraw;
    
    private void Awake()
    {
        afpsCounter = FindObjectOfType<AFPSCounter>();

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

        // gridW = 2;
        // gridH = 2;
    }

    private void Start()
    {
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup]
    void AddH()
    {
        gridH += 2;
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup]
    void SubH()
    {
        if (gridH >= 2)
        {
            gridH -= 2;
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup]
    void AddW()
    {
        gridW += 2;
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup]
    void SubW()
    {
        if (gridW >= 2)
        {
            gridW -= 2;
            if (isDelay)
            {
                return;
            }
            Generate();
        }
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    void AddHS()
    {
        h += 2;
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    void SubHS()
    {
        if (h >= 2)
        {
            h -= 2;
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    void AddWS()
    {
        w += 2;
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    void SubWS()
    {
        if (w >= 2)
        {
            w -= 2;
            if (isDelay)
            {
                return;
            }
            Generate();
        }
    }

    [HorizontalGroup("B")]
    [Button(ButtonSizes.Medium)]
    void Generate()
    {
        Clear();
        if (null == item)
        {
            return;
        }

        var numW = isFillScreen ? gridW : w;
        var numH = isFillScreen ? gridH : h;

        if (null != textInfoGridSize)
        {
            textInfoGridSize.text = $"{gridW * gridH}={gridW}x{gridH}";
        }

        if (null != textInfoRealSize)
        {
            textInfoRealSize.text = $"{w * h}={w}x{h}";
        }

        if (null != textTotal)
        {
            textTotal.text = $"{(float) (w * h) / (gridW * gridH):F2}={w * h}/{gridW * gridH}";
        }

        var rectTransform = layout.GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        var width = rect.width / gridW;
        var height = rect.height / gridH;

        for (int i = 0; i < layer; i++)
        {
            for (int col = 0; col < numW; col++)
            {
                for (int row = 0; row < numH; row++)
                {
                    var grid = Instantiate(item, layout.transform);
                    var gridRect = grid.GetComponent<RectTransform>();
                    gridRect.sizeDelta = new Vector2(width, height);
                    gridRect.anchoredPosition = new Vector2(col * width, row * height);

                    var img = grid.GetComponent<Image>();
                    var flag = (col % 2 == 0) == (row % 2 != 0);
                    img.color = flag ? mainColor : subColor;

                    var mat = img.material;
                    if (null != mat)
                    {
                        img.material = new Material(img.material.shader);
                    }

                    if (isSetSprite)
                    {
                        img.sprite = sprites[Random.Range(0, sprites.Length)];
                    }

#if UNITY_EDITOR
                    img.name = $"{col}_{col % 2} {row}_{row % 2} {mat.GetHashCode()}";
#endif
                }
            }
        }
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("B")]
    void Clear()
    {
        layout.transform.DeleteAllChildren();
    }

    public Texture2D tex;

    [Button()]
    [HorizontalGroup("B")]
    public void Record()
    {
        StartCoroutine(UploadPNG());
    }

    IEnumerator UploadPNG()
    {
        if (null != asset)
        {
            asset.isOverlay = true;
        }

        Shader.SetGlobalFloat("_OverlayParam", minValue);
        
        if (null != operaCanvas)
        {
            operaCanvas.gameObject.SetActive(false);
        }

        if (null != afpsCounter)
        {
            afpsCounter.OperationMode = OperationMode.Disabled;
        }

        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        if (null == tex)
        {
            tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        }

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        if (null != operaCanvas)
        {
            operaCanvas.gameObject.SetActive(true);
        }

        if (null != afpsCounter)
        {
            afpsCounter.OperationMode = OperationMode.Normal;
        }

        if (null != asset)
        {
            asset.isOverlay = false;
        }

        var pixels = tex.GetPixels();
        // Debug.Log($"{width},{height}   {width * height}   {pixels.Length}");

        var allRed = pixels.Length;
        var red = 0f;

        var l = pixels.Length / 10;
        var circle = 0;
        for (var i = 0; i < pixels.Length; i++)
        {
#if UNITY_EDITOR
            circle++;
            if (circle > l)
            {
                circle = 0;
                var f = (float)i / pixels.Length;
                EditorUtility.DisplayProgressBar("For every pixel", $"{i}/{pixels.Length}", f);        
#endif
            }
            
            var pix = pixels[i];
            var r = pix.r;
            var round = Mathf.RoundToInt(r / minValue);
            var a = round;
            red += a;
            if (Random.Range(0f, 1f) > .001f)
            {
                Debug.Log($"{i}: {a}");
            }
        }

#if UNITY_EDITOR
        EditorUtility.ClearProgressBar();
#endif

        
        float pect = red / allRed;
        overdraw = $"{pect:P2}" ;
        Debug.Log($"pect: {red}/{allRed} = {pect}");
    }
}