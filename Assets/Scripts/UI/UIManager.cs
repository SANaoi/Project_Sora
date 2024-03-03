using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public static UIManager _instance;
    private Transform _uiRoot;
    private Dictionary<string, string> pathDict;
    private Dictionary<string, GameObject> prefabDict;
    public Dictionary<string, BasePanel> panelDict;

    // 需在初始化就生成的UI界面
    public ItemsInfo ItemsInfo;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    // private void Awake()
    // {
    //     _instance = (UIManager)this;
    // }
    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private UIManager()
    {
        InitDicts();
        InitMainUI();
    }
    private void InitMainUI()
    {
        ItemsInfo = GetUIGameObject("ItemsInfo").GetComponent<ItemsInfo>();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        // 配置路径
        pathDict = new Dictionary<string, string>()
        {
            {UIConst.PackagePanel, "Package/PackagePanel"},

            {UIConst.Item, "MainUI/Item"},
            {UIConst.ItemsInfo, "MainUI/ItemsInfo"},
            {UIConst.GunInfo, "MainUI/GunInfo"},
            {UIConst.ItemDetail, "MainUI/ItemDetail"},
        };
    }

    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        if(panelDict.TryGetValue(name, out panel))
        {
            return null;
        }

        string path = "";
        if(!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("界面显示错误或未配置路径：" + name);
            return null;
        }

        GameObject panelPrefab = null;
        if(!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath);
            prefabDict.Add(name, panelPrefab);
        }   

        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;
    }

    public bool ClosePanel(string name)
    {   
        PlayerMoveControls.Instance.Player.Aiming.performed += AimingController.Instance.SwitchCameraParameter;

        BasePanel panel = null;
        if(!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面未打开" + name);
            return false;
        }
        panel.ClosePanel(name);
        return true;
    }

    public GameObject GetUIGameObject(string name)
    {   
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("未配置路径：" + name);
            return null;
        }
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath);
            prefabDict.Add(name, panelPrefab);
        }
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        return panelObject;
    }
}
public class UIConst
{
    // 配置名称
    public const string PackagePanel = "PackagePanel";
    public const string Item = "Item";
    public const string ItemDetail = "ItemDetail";
    public const string ItemsInfo = "ItemsInfo";
    public const string GunInfo = "GunInfo";
}
