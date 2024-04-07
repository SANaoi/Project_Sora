using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
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
                _instance = new GameObject("UIManager").AddComponent<UIManager>();
            }
            return _instance;
        }
    }
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

    private void Awake()
    {
        print(this.name + " Awake——------------");
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        // 否则，设置这个实例为单例
        _instance = this;


    }
    private void OnEnable()
    {
        print(this.name + " OnEnable------------");
        InitDicts();
        // InitMainUI();

    }
    private void OnDisable()
    {
        print(this.name + " OnDisable------------");
    }
    public void InitMainUI()
    {
        OpenPanel("ItemsInfo").GetComponent<ItemsInfo>();
    }
    public void RefreshManager()
    {
        EventSystem eventSystem = FindAnyObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }
        InitDicts();
        InitMainUI();
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
            {UIConst.DialogBox, "MainUI/DialogBox"},
            {UIConst.ItemDetail, "MainUI/ItemDetail"},
            {UIConst.SelectBox, "MainUI/DialogSelectBox"},
            
            {UIConst.HealthBarUI, "CharacterUI/Bar Holder"},
            {UIConst.PlayerMainUI, "CharacterUI/PlayerMainUI"},

            {UIConst.TasksPanel, "TasksPanel/TasksPanel"},

            {UIConst.AimImage, "AimImage"},

            {UIConst.AudioUIManager, "AudioPanel/MainMenu"}
        };
    }

    public BasePanel OpenPanel(string name, bool isLockInput = false)
    {
        if (isLockInput)
        {
            EventCenter.Instance.EventTrigger(EventConst.LogoutInputSystem);
        }
        BasePanel panel = null;
        if(panelDict.TryGetValue(name, out panel))
        {
            return panel;
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

    public bool ClosePanel(string name, bool isLockInput = false)
    {   
        if (isLockInput)
        {
            EventCenter.Instance.EventTrigger(EventConst.ActiveInputSystem);
        }

        BasePanel panel = null;
        if(!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面未打开" + name);
            return false;
        }
        panel.ClosePanel(name);
        if(panelDict.ContainsKey(name))
        {
            panelDict.Remove(name);
        }
        return true;
    }

    public BasePanel OpenPanel(string name, Transform UIRoot)
    {   
        BasePanel panel = null;
        if(panelDict.TryGetValue(name, out panel))
        {
            
            return panel;
        }
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
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        if (panel == null)
        {
            Debug.Log("null");
            Debug.Log(panelDict.Keys);
        }
        Debug.Log(name+"  将被生成" );
        return panel;
    }

    public GameObject OpenGameObject(string name, Transform UIRoot)
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
    private void OnDestroy()
    {
        // 当 GameManager 实例被销毁时，清理静态实例引用
        if (_instance == this)
        {
            _instance = null;
        }
    }


}
public class UIConst
{
    // 配置名称

    // 背包
    public const string PackagePanel = "PackagePanel";

    // 场景
    public const string Item = "Item";
    public const string ItemDetail = "ItemDetail";
    public const string ItemsInfo = "ItemsInfo";
    public const string GunInfo = "GunInfo";
    public const string HealthBarUI = "HealthBarUI";
    public const string PlayerMainUI = "PlayerMainUI";
    public const string DialogBox = "DialogBox";
    public const string SelectBox = "DialogSelectBox";
    public const string TasksPanel = "TaskPanel";
    public const string AimImage = "AimImage";

    public const string AudioUIManager = "AudioUIManager"; 
}
