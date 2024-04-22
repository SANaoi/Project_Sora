using System;
using System.Collections;
using System.Collections.Generic;
using aoi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMoveControls inputActions;
    private PackageTable_SO packageTable;
    public CurrentTask_SO currentTask;
    public List<int> CompleteTasksID;
    public TaskData_SO taskData;
    public PlayerManager playerManager;
    public GetSceneItems getSceneItems;
    public GameObject AudioManagerPrefab;
    public GameObject Team;
    public static bool applicationIsQuitting = false;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
				if (applicationIsQuitting) {
					//如果销毁了则直接返回，不能再创建
					return _instance;
				}
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
        InitBaseGameObject();

        
    }
    

    void OnEnable()
    {
        print(this.name + "  OnEnable");
        inputActions.Enable();
        inputActions.Player.OpenPackage.performed += GetOpenPackageInput;
        inputActions.Player.ESC.performed += SwitchMenuInput;
    }
    
    void Disable()
    {
        print(this.name + "  Disable");
        inputActions.Disable();
    }
    
    private void Start()
    {
        InitTaskData();
        print(this.name + "  Start");
        
    }
    # region 场景初始化
    public void InitTaskData()
    {
        UserData userData = GetUserData();
        CompleteTasksID = userData.completedTasks;
        currentTask.TaskDetailsList.Clear();
        foreach (TaskDetails taskDetails in taskData.TaskDetailsList)
        {
            if (userData.currentTasks.Contains(taskDetails.taskID))
            {
                currentTask.TaskDetailsList.Add(taskDetails);
            }
        }
    }
    public UserData GetUserData()
    {
        UserData data = LocalConfig.LoadUserData("test_aoi");
        if (data == null)
        {
            CreateNewLocalConfig();
            return LocalConfig.LoadUserData("test_aoi");
        }
        return data;
    }
    public void InitPlayerManager()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        playerManager.UpdatePackageLocalData();
    }

    public void InitBaseInput()
    {
        BasePanel a = UIManager.Instance.OpenPanel(UIConst.PlayerMainUI);
        BasePanel b = UIManager.Instance.OpenPanel(UIConst.GunInfo);
        if (a) a.gameObject.SetActive(false);
        if (b) b.gameObject.SetActive(false);
        inputActions.Disable();
    }

    public void InitBaseGameObject()
    {
        if (inputActions == null)  inputActions = new PlayerMoveControls();

        TransitionPoint transitionPoint = FindAnyObjectByType<TransitionPoint>();
        if (transitionPoint != null)
        {
            Vector3 position = transitionPoint.transform.position;
            Instantiate(Team,  position, Quaternion.identity);
        }
        else
        {
            Instantiate(Team);
        }
        AudioManager audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager == null) Instantiate(AudioManagerPrefab);
        
        playerManager = FindAnyObjectByType<PlayerManager>();

        getSceneItems = FindAnyObjectByType<GetSceneItems>();
    }

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

    public PackageLocalItem GetPackageTableItemByUId(string uid) // 获取指定背包道具
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

    public void UsePackageItemByUid(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        // foreach(PackageLocalItem item in packageDataList)
        // {
        //     if(item.uid == uid)
        //     {
        //         item.num -= 1;
        //     }
        //     if (item.num <= 0)
        //     {
        //         packageDataList.Remove(item);
        //     }
        // }
        for (int i = 0; i < packageDataList.Count; i++)
        {
            if (packageDataList[i].uid == uid)
            {
                packageDataList[i].num -= 1;
                if (packageDataList[i].num <= 0)
                {
                    packageDataList.Remove(packageDataList[i]);
                }
                PackageLocalData.Instance.SavePackage();
                return;
            }
        }
        return;
    }


    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }

    public void GetOpenPackageInput(InputAction.CallbackContext context)
    {   
        BasePanel panel = null;
        if(!UIManager.Instance.panelDict.TryGetValue(UIConst.PackagePanel, out panel))
        {
            UIManager.Instance.OpenPanel(UIConst.PackagePanel, true);
        }
        else
        {
            UIManager.Instance.ClosePanel(UIConst.PackagePanel, true);
        }
    }

    private void SwitchMenuInput(InputAction.CallbackContext context)
    {
        BasePanel panel = null;
        if(!UIManager.Instance.panelDict.TryGetValue(UIConst.AudioUIManager, out panel))
        {
            UIManager.Instance.OpenPanel(UIConst.AudioUIManager, true);
        }
        else
        {
            UIManager.Instance.ClosePanel(UIConst.AudioUIManager, true);
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
    public void RemovePackageTableItemByList(List<PackageLocalItem> Removeitems, int numberOfRemove)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        if (Removeitems == null) return;
        
        foreach (PackageLocalItem item in Removeitems)
        {
            items.Remove(item);
            numberOfRemove -= 1;
            if (numberOfRemove == 0) break;
            
        }
        PackageLocalData.Instance.SavePackage();
    }

    // DiscountNum 同id道具所减少的总量
    public void DiscountPackageLocalItemsNumById(int id ,int DiscountNum)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        List<PackageLocalItem> Removeitems = new();
        foreach (PackageLocalItem item in items)
        {
            if (item.id == id)
            {
                if (item.num > DiscountNum) // 判断当前道具的数量是否大于需要减少的量
                {
                    item.num -= DiscountNum;
                    RemovePackageTableItemByList(Removeitems, Removeitems.Count);
                    return;
                }
                else
                {
                    DiscountNum -= item.num;
                    Removeitems.Add(item);
                }
            }
        }
        RemovePackageTableItemByList(Removeitems, Removeitems.Count);
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
        if (taskDetails.taskType == TaskType.击杀)
        {
            for (int i = 0; i < taskDetails.DefaultList.Count; i++)
            {
                Default DefaultList = taskDetails.DefaultList[i];
                DefaultList.CurrentKill = 0;
            }
        }
        else if (taskDetails.taskType == TaskType.收集)
        {   
            for (int i = 0; i < taskDetails.CollectList.Count; i++)
            {
                Collect CollectList = taskDetails.CollectList[i];
                CollectList.CurrentNumber = 0;
            }
        }
        else if (taskDetails.taskType == TaskType.歼灭)
        {

        }
        else if (taskDetails.taskType == TaskType.存活)
        {

        }
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
            for (int i = 0; i < taskDetail.DefaultList.Count; i++)
            {
                Default defaultList = taskDetail.DefaultList[i];
                if (defaultList.CurrentKill != defaultList.killTarget) { return false; }
            }
        }
        else if (taskDetail.taskType == TaskType.收集)
        {   
            // TODO 检测背包是否满足数量
            
            for (int i = 0; i < taskDetail.CollectList.Count; i++)
            {
                Collect CollectList = taskDetail.CollectList[i];

                List<PackageLocalItem> Removeitems = PackageLocalData.Instance.GetListWithHaveSameID(CollectList.ItemInfo.id);
                if (Removeitems.Count >= CollectList.CollectTarget) RemovePackageTableItemByList(Removeitems, CollectList.CollectTarget);
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
        aoi.TaskDetails taskDetail = currentTask.TaskDetailsList.Find(i => i.taskID == ID);
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

    # endregion

    # region 本地存储相关

    public void SaveCurrentLocalConfig()
    { 
        // 保存玩家数据
        TeamController team = FindObjectOfType<TeamController>();
        UserData userData= new UserData();
        userData.name = "test_aoi";
        if (team)
        {
            PlayerManager[] players = team.charactersList;
            List<string> teamCharacterNames = new();
            List<int> health = new();
            List<int> magazineAmmo = new();
            foreach (PlayerManager p in players)
            {
                teamCharacterNames.Add(p.gameObject.name);
                health.Add(p.GetComponent<CharacterStats>().CurrentHealth);
                magazineAmmo.Add(p.GetComponent<ShootController>().MagazineAmmo);
            }
            userData.teamCharacterNames = teamCharacterNames;
            userData.health = health;
            userData.magazineAmmo = magazineAmmo;
        }
        else
        {
            Debug.LogError("未保存玩家数据");
            return;
        }
        userData.currentScene = SceneManager.GetActiveScene().name;
        foreach (TaskDetails taskDetail in currentTask.TaskDetailsList)
        {  
            if (!userData.currentTasks.Contains(taskDetail.taskID))
                userData.currentTasks.Add(taskDetail.taskID);
        }
        foreach (int i in CompleteTasksID)
        {
            if (!userData.completedTasks.Contains(i))
            {
                userData.completedTasks.Add(i);
            }
        }
        LocalConfig.SaveUserData(userData);
    }
    public void CreateNewLocalConfig()
    {
        TeamController team = FindObjectOfType<TeamController>();
        UserData userData= new UserData();
        userData.name = "test_aoi";
        if (team)
        {
            PlayerManager[] players = team.charactersList;
            List<string> teamCharacterNames = new();
            List<int> health = new();
            List<int> magazineAmmo = new();
            foreach (PlayerManager p in players)
            {
                teamCharacterNames.Add(p.gameObject.name);
                health.Add(p.GetComponent<CharacterStats>().characterData.maxHealth);
                magazineAmmo.Add(p.GetComponent<ShootController>().ShootConfig.Capacity);
            }
            userData.teamCharacterNames = teamCharacterNames;
            userData.health = health;
            userData.magazineAmmo = magazineAmmo;
        }
        else
        {
            Debug.LogError("未保存玩家数据");
            return;
        }
        CompleteTasksID.Clear();
        currentTask.TaskDetailsList.Clear();
        userData.currentScene = null;
        userData.currentTasks = new();
        userData.completedTasks = new();
        LocalConfig.SaveUserData(userData);
    }
    // 获取本地文件里指定角色的列表下标
    public int GerPlayerIndex(string name)
    {
        return GetUserData().teamCharacterNames.IndexOf(name);
    }
    # endregion

    protected virtual void OnDestroy() 
    {
        // SaveCurrentLocalConfig();
		applicationIsQuitting = true;
	}


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

public class AudioVolumeProcessor
{
    public static float AudioVolumeCalculate(Vector3 thisPosition, Vector3 targetPosition, float max_value, float min_value, float max_distance)
    {
        // 计算距离
        float distance = Vector3.Distance(thisPosition, targetPosition);
        // 超过最大值返回最小音量
        if (distance > max_distance)
        {
            return min_value;
        }
        // 计算音量
        float volume = max_value * (1 - distance / max_distance);
        return Mathf.Max(volume, min_value);
    }
}