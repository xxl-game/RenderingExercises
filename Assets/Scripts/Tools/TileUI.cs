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
    [MinValue(2)]
    [HorizontalGroup("Opera/grid")]
    [LabelWidth(50)]
    public int gridW;

    [BoxGroup("Opera")]
    [MinValue(2)]
    [HorizontalGroup("Opera/grid")]
    [LabelWidth(50)]
    public int gridH;

    [MinValue(2)]
    [BoxGroup("Opera")]
    [HorizontalGroup("Opera/line")]
    [LabelWidth(50)]
    public int w;

    [MinValue(2)]
    [BoxGroup("Opera")]
    [HorizontalGroup("Opera/line")]
    [LabelWidth(50)]
    public int h;

    [BoxGroup("Opera")]
    public bool isSetSprite = true;

    [BoxGroup("Opera")]
    public bool isFillScreen = true;
    
    public bool isDelay = false;
    
    [MinValue(1)]
    [BoxGroup("Opera")]
    [PropertyOrder(-1)]
    [LabelWidth(50)]
    public int layer = 1;

    [Range(0.01f, 0.2f)]
    public float minValue = 0.1f;
    
    [DisplayAsString]
    [GUIColor(0f, 1f, 0f, 1f)]
    public string overdraw;

    [ShowInInspector]
    [DisplayAsString]
    private string pectOnPaper;
    
    [HideInInspector]
    public List<Material> materials;

    [HideInInspector]
    public List<Material> usedMaterials;

    [HorizontalGroup("引用/Color")]
    [HideLabel]
    [PropertyOrder(100)]
    public Color mainColor;

    [HorizontalGroup("引用/Color")]
    [HideLabel]
    [PropertyOrder(100)]
    public Color subColor;
    
    [FoldoutGroup("引用", Order = 10000)]
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
    public Button addL;

    [FoldoutGroup("引用")]
    public Button subL;

    [FoldoutGroup("引用")]
    public Button toggle;

    [FoldoutGroup("引用")]
    public Button btnExecute;
    
    [FoldoutGroup("引用")]
    public Text textInfoGridSize;
    
    [FoldoutGroup("引用")]
    public Text textInfoRealSize;

    [FoldoutGroup("引用")]
    public Text textTotal;

    [FoldoutGroup("引用")]
    public Canvas operaCanvas;
    
    [FoldoutGroup("引用")]
    public CatlikeRenderPipelineAsset asset;
    
    [PropertyOrder(1000)]
    public Sprite[] sprites;
    
    private AFPSCounter afpsCounter;
    
    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        isDelay = true;
#endif
        
        afpsCounter = FindObjectOfType<AFPSCounter>();

        materials = new List<Material>();
        usedMaterials = new List<Material>();
        Application.targetFrameRate = 30;
        if (null != addW)
        {
            addW.onClick.AddListener(AddWidth);
        }

        if (null != subW)
        {
            subW.onClick.AddListener(SubWidth);
        }
        
        if (null != addH)
        {
            addH.onClick.AddListener(AddHeight);
        }

        if (null != subH)
        {
            subH.onClick.AddListener(SubHeight);
        }
        
        if (null != addWS)
        {
            addWS.onClick.AddListener(AddCol);
        }

        if (null != subWS)
        {
            subWS.onClick.AddListener(SubCol);
        }

        if (null != addHS)
        {
            addHS.onClick.AddListener(AddRow);
        }

        if (null != subHS)
        {
            subHS.onClick.AddListener(SubRow);
        }
        
        if (null != addL)
        {
            addL.onClick.AddListener(AddL);   
        }

        if (null != subL)
        {
            subL.onClick.AddListener(SubL);   
        }

        if (null != toggle)
        {
            toggle.onClick.AddListener(Toggle);
        }

        if (null != btnExecute)
        {
            btnExecute.onClick.AddListener(Generate);
        }
        
        // gridW = 2;
        // gridH = 2;
    }

    private void Start()
    {
        // Generate();
    }

    [PropertyOrder(11)]
    [Button("+height", ButtonSizes.Medium)]
    [HorizontalGroup]
    void AddHeight()
    {
        gridH += 2;
        UpdateText();
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("-height", ButtonSizes.Medium)]
    [HorizontalGroup]
    [PropertyOrder(11)]
    void SubHeight()
    {
        if (gridH > 2)
        {
            gridH -= 2;
            UpdateText();
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("+width", ButtonSizes.Medium)]
    [HorizontalGroup]
    [PropertyOrder(10)]
    void AddWidth()
    {
        gridW += 2;
        UpdateText();
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("-width", ButtonSizes.Medium)]
    [HorizontalGroup]
    [PropertyOrder(10)]
    void SubWidth()
    {
        if (gridW > 2)
        {
            gridW -= 2;
            UpdateText();
            if (isDelay)
            {
                return;
            }
            Generate();
        }
    }

    [Button("+row", ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    [PropertyOrder(21)]
    void AddRow()
    {
        if (h < gridH)
        {
            h += 2;
            UpdateText();
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("-row", ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    [PropertyOrder(21)]
    void SubRow()
    {
        if (h > 2)
        {
            h -= 2;
            UpdateText();
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("+col", ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    [PropertyOrder(20)]
    void AddCol()
    {
        if (w < gridW)
        {
            w += 2;
            UpdateText();
        }
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button("-col", ButtonSizes.Medium)]
    [HorizontalGroup("s")]
    [PropertyOrder(20)]
    void SubCol()
    {
        if (w > 2)
        {
            w -= 2;
            UpdateText();
            if (isDelay)
            {
                return;
            }
            Generate();
        }
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("L")]
    void AddL()
    {
        layer++;
        UpdateText();
        if (isDelay)
        {
            return;
        }
        Generate();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("L")]
    void SubL()
    {
        if (layer > 0)
        {
            layer--;
            UpdateText();
            if (isDelay)
            {
                return;
            }
            Generate();
        }
    }

    void Toggle()
    {
        asset.isOverlay = !asset.isOverlay;
        Debug.Log($"overlay:{asset.isOverlay}");
        if (null != toggle)
        {
            toggle.GetComponentInChildren<Text>().text = asset.isOverlay.ToString();
        }
    }

    void UpdateText()
    {
        if (null != textInfoGridSize)
        {
            textInfoGridSize.text = $"{gridW * gridH * layer}={gridW}x{gridH}x{layer}";
        }

        if (null != textInfoRealSize)
        {
            textInfoRealSize.text = $"{w * h * layer}={w}x{h}x{layer}";
        }
    }

    [HorizontalGroup("B")]
    [Button(ButtonSizes.Medium)]
    [PropertyOrder(50)]
    void Generate()
    {
        UpdateText();
        
        Clear();
        if (null == item)
        {
            return;
        }
        
        var numW = isFillScreen ? gridW : w;
        var numH = isFillScreen ? gridH : h;
        
        pectOnPaper = $"{(float) (w * h * layer) / (gridW * gridH):P2}";

        UpdatePectText();

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
        
        Record();
    }

    [Button(ButtonSizes.Medium)]
    [HorizontalGroup("B")]
    [PropertyOrder(50)]
    void Clear()
    {
        layout.transform.DeleteAllChildren();
    }

    public Texture2D tex;
    
    void UpdatePectText()
    {
        if (null != textTotal)
        {
            textTotal.text = $"{overdraw}/{pectOnPaper}";
        }
    }
    
    [Button()]
    [HorizontalGroup("B")]
    [PropertyOrder(50)]
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
        
        var pixels = tex.GetPixels();
        Debug.Log($"{width},{height}={width * height}   {pixels.Length}");

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
            }
#endif
            
            var pix = pixels[i];
            var r = pix.r;
            var round = Mathf.RoundToInt(r / minValue);
            var a = round;
            red += a;
        }

#if UNITY_EDITOR
        EditorUtility.ClearProgressBar();
#endif

        
        float pect = red / allRed;
        overdraw = $"{pect:P2}" ;
        Debug.Log($"pect: {red}/{allRed} = {pect}");
        
        UpdatePectText();

        if (null != asset)
        {
            asset.isOverlay = false;
        }
        
#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
    }
}