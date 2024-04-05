using System;
using System.Collections;
using System.Collections.Generic;
using aoi;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMoveControls inputActions;
    private PackageTable_SO packageTable;
    public CurrentTask_SO currentTask;
    public TaskData_SO taskData;
    public UIManager uIManager;
    private static GameManager _instance;
    private PlayerManager playerManager;
    public GameObject playerPrefab;


    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 寻找场景中现有的 GameManager 实例
                _instance = FindObjectOfType<GameManager>();
                
        
                // 如果场景中没有 GameManager 实例，则创建一个
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        // 如果已经有了实例，并且不是这个实例，销毁这个实例
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // 否则，设置这个实例为单例
        _instance = this;
        // 可以在这里进行其他的初始化操作

        print(this.name + "  Awake");
        uIManager = UIManager.Instance;
        if (inputActions == null)  inputActions = new PlayerMoveControls();
        playerManager = FindAnyObjectByType<PlayerManager>();
        
    }
    

    void OnEnable()
    {
        print(this.name + "  OnEnable");
        inputActions.Enable();
        inputActions.Player.OpenPackage.performed += GetOpenPackageInput;
    }
    
    void Disable()
    {
        print(this.name + "  Disable");
        inputActions.Player.OpenPackage.performed -= GetOpenPackageInput;
        inputActions.Disable();
    }
    
    private void Start()
    {
        print(this.name + "  Start");
        // UIManager.Instance.OpenPanel(UIConst.ItemsInfo);
        // UIManager.Instance.OpenPanel(UIConst.PlayerMainUI);
        // UIManager.Instance.OpenPanel(UIConst.GunInfo);
    }
    # region 场景初始化

    # endregion

    #region 背包的基础功能
    public PackageTable_SO GetPackageTable()
    {
        if(packageTable == null)
        {
            packageTable = Resources.Load<PackageTable_SO>("Data/PackageData/PackageTable");
        }
        return packageTable;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }

    public PackageTableItem GetPackageTableItemById(int id)
    {
        List<PackageTableItem> packageDataList = GetPackageTable().DataList;
        foreach(PackageTableItem item in packageDataList)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    public PackageLocalItem GetPackageTableItemByUId(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach(PackageLocalItem item in packageDataList)
        {
            if(item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }


    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }

    public void GetOpenPackageInput(InputAction.CallbackContext context)
    {
        if(!UIManager.Instance.OpenPanel(UIConst.PackagePanel))
        {
            UIManager.Instance.OpenPanel(UIConst.PackagePanel);
        }
    }
    
    public int GetPackageLocalItemsNumById(int id)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        int res = 0;
        foreach (PackageLocalItem item in items)
        {
            if (item.id == id)
            {
                res += item.num;
            }
        }
        return res;
    }
    public void RemovePackageTableItemByList(List<PackageLocalItem> Removeitems)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        if (Removeitems == null) return;
        foreach (PackageLocalItem item in Removeitems)
        {
            items.Remove(item);
        }
        PackageLocalData.Instance.SavePackage();
    }
    public void DiscountPackageLocalItemsNumById(int id ,int DiscountNum)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        List<PackageLocalItem> Removeitems = new();
        foreach (PackageLocalItem item in items)
        {
            if (item.id == id)
            {
                if (item.num > DiscountNum)
                {
                    item.num -= DiscountNum;
                    RemovePackageTableItemByList(Removeitems);
                    return;
                }
                else
                {
                    DiscountNum -= item.num;
                    Removeitems.Add(item);
                    print(2);
                }
            }
        }
        RemovePackageTableItemByList(Removeitems);
        PackageLocalData.Instance.SavePackage();
    }
    #endregion

    #region 任务系统
    public TaskDetails GetTaskDetailByID(int Id)
    {
        return currentTask.TaskDetailsList.Find(i => i.taskID == Id);
    }


    public void IncreaseTestTaskProgress(int targetId)
    {
        string textContent = "";
        int currentID = TaskPanel.CurrentID;
        aoi.TaskDetails taskDetail = currentTask.TaskDetailsList.Find(i => i.taskID == currentID);
        if (taskDetail != null)
        {
            TaskPanel.IncreaseTaskProgress(taskDetail, targetId, ref textContent);
            (UIManager.Instance.OpenPanel(UIConst.PlayerMainUI) as PlayerMainUI).RefreshTaskInfo();
        }
    }

    public void AddTaskToCurrentTask(int ID)
    {
        TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
        currentTask.TaskDetailsList.Add(taskDetails);
    }
    public void RemoveTaskToCurrentTask(int ID)
    {
        TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
        currentTask.TaskDetailsList.Remove(taskDetails);
    }
    public bool IsCompleteTask(TaskDetails taskDetail)
    {
        if (taskDetail.taskType == TaskType.击杀)
        {
            for (int i = 0; i < taskDetail.DefaultLsit.Count; i++)
            {
                Default defaultList = taskDetail.DefaultLsit[i];
                if (defaultList.CurrentKill != defaultList.killTarget) { return false; }
            }
        }
        else if (taskDetail.taskType == TaskType.收集)
        {   
            // TODO 检测背包是否满足数量
            
            for (int i = 0; i < taskDetail.CollectList.Count; i++)
            {
                Collect CollectList = taskDetail.CollectList[i];
                if (CollectList.CurrentNumber != CollectList.CollectTarget) { return false; }

                List<PackageLocalItem> Removeitems = PackageLocalData.Instance.GetListWithHaveSameID(CollectList.ItemInfo.id);
                if (Removeitems.Count >= CollectList.CollectTarget) RemovePackageTableItemByList(Removeitems);
                else return false;
            }
            
        }
        else if (taskDetail.taskType == TaskType.歼灭)
        {

        }
        else if (taskDetail.taskType == TaskType.存活)
        {

        }
        
        return true;
    }

    public void GetTaskReward(TaskDetails taskDetail)
    {
        
        for (int i = 0; i < taskDetail.remuneration.Count; i++)
        {
            ItemInfo_SO reward = taskDetail.remuneration[i];
            PackageLocalData.Instance.AddPackageLocalItem(reward);
        }
    }

    public void ReMoveCurrentTask(TaskDetails taskDetail)
    {
        currentTask.TaskDetailsList.Remove(taskDetail);
        TaskPanel.CurrentID = 0;
    }

    public void PostTask(int ID)
    {
        aoi.TaskDetails taskDetail = GameManager.Instance.currentTask.TaskDetailsList.Find(i => i.taskID == ID);
        if (taskDetail != null)
        {
            IsCompleteTask(taskDetail);
            GetTaskReward(taskDetail);
            ReMoveCurrentTask(taskDetail);
            PackageLocalData.Instance.SavePackage();
            playerManager.RefreshGunInfo();
            (UIManager.Instance.OpenPanel(UIConst.PlayerMainUI) as PlayerMainUI).RefreshTaskInfo();
            Debug.Log("完成任务");
        }
    }
    #endregion

    # region UI相关
    // public void RefreshUI()
    // {
    //     PlayerManager.Instance.RefreshGunInfo();
    // }

    # endregion
}

public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageTableItem x = GameManager.Instance.GetPackageTableItemById(a.id);
        PackageTableItem y = GameManager.Instance.GetPackageTableItemById(b.id);
        int idComparison = y.id.CompareTo(x.id);
        if (idComparison == 0)
        {
            return b.num.CompareTo(a.num);
        }
        return idComparison;
    }
}
