using System.Collections;
using System.Collections.Generic;
using aoi;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public PlayerMoveControls inputActions;
    private PackageTable_SO packageTable;
    public CurrentTask_SO currentTask;
    public TaskData_SO taskData;

    // public ShootController shootController;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new PlayerMoveControls();
        inputActions.Player.OpenPackage.performed += GetOpenPackageInput;
        
        // UIManager.Instance.OpenPanel(UIConst.DialogBox);
        // shootController = new ShootController();
    }
    
    void OnEnable()
    {
        inputActions.Enable();
    }

    void Disable()
    {
        inputActions.Disable();
    }
    
    private void Start()
    {
    }

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
    #endregion

    #region 任务系统
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
            for (int i = 0; i < taskDetail.CollectLsit.Count; i++)
            {
                Collect CollectLsit = taskDetail.CollectLsit[i];
                if (CollectLsit.CurrentNumber != CollectLsit.CollectTarget) { return false; }
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
    #endregion
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
