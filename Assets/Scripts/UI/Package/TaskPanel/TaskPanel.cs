using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using aoi;
using System.Security.Cryptography;
public class TaskPanel : BasePanel
{
    public TaskData_SO taskData;
    public CurrentTask_SO currentTask;
    private Transform UICancelButton;
    private Transform UITasksList;
    private Transform UITaskName;
    private Transform UITaskDescription;
    private Transform UITaskDemand;
    private Transform UITaskReward;
    private Transform UITaskRewardPreview;
    private Transform UITaskListContent;
    private Transform UITaskTrailButton;

    public GameObject TaskCellPrefab;
    public GameObject RewardPreviewPrefab;

    private PlayerMainUI playerMainUI;
    // private static int _currentID;
    public static int CurrentID;
    public Dictionary<int , int> TaskDict;
    private int _chooseID;
    public int chooseID
    {
        get
        {
            return _chooseID;
        }
        set
        {
            _chooseID = value;
            RefreshDetail();
            RefreshReward();
        }
    }

    private void Awake()
    {
        InitUI();
        InitClik();
    }

    private void OnEnable() 
    {
        RefreshUI();
    }

    private void InitUI()
    {
        InitUIName();

        RectTransform scrollContent = UITaskListContent as RectTransform;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < UITaskReward.childCount; i++)
        {
            Destroy(UITaskReward.GetChild(i).gameObject);
        }

        UITaskName.GetComponent<Text>().text = "";
        UITaskDescription.GetComponent<Text>().text = "";
        UITaskDemand.GetComponent<Text>().text = "";


    }

    private void InitUIName()
    {
        UICancelButton = transform.Find("CenterTop/Cancel");
        UITasksList = transform.Find("Center/TasksList/Scroll View");
        UITaskName = transform.Find("Center/TaskDetail/Top/TaskName");
        UITaskDescription = transform.Find("Center/TaskDetail/Center/Description");
        UITaskDemand = transform.Find("Center/TaskDetail/Center/Demand");
        UITaskReward = transform.Find("Center/TaskDetail/Bottom");
        UITaskRewardPreview = transform.Find("Center/TaskDetail/Center/RewardPreview");
        UITaskTrailButton = transform.Find("Bottom/Trail");
        UITaskListContent = UITasksList.GetComponent<ScrollRect>().content;

        playerMainUI = UIManager.Instance.OpenPanel(UIConst.PlayerMainUI) as PlayerMainUI;
    }

    private void RefreshUI()
    {
        RefreshTaskList();
        RefreshDetail();
        RefreshReward();
    }

    private void RefreshDetail()
    {
        if (chooseID != 0)
        {   
            TaskDetails taskDetail = currentTask.TaskDetailsList.Find(i => i.taskID == chooseID);
            UITaskName.GetComponent<Text>().text = taskDetail.taskName;
            UITaskDescription.GetComponent<Text>().text = taskDetail.taskDescription;
            string textContent = "";
            CheckTaskType(taskDetail, ref textContent);
            UITaskDemand.GetComponent<Text>().text = textContent;
        }
    }

    
    private void RefreshTaskList()
    {
        RectTransform scrollContent = UITaskListContent as RectTransform;
        if (currentTask.TaskDetailsList.Count != 0)
        {
            for (int i = 0; i < currentTask.TaskDetailsList.Count; i++)
            {
                TaskDetails taskDetail = currentTask.TaskDetailsList[i];
                Transform taskCell = Instantiate(TaskCellPrefab.transform, scrollContent) as Transform;
                taskCell.GetComponent<TaskCell>().Refresh(taskDetail.taskName, taskDetail.taskID, this);
            }
        }
    }

    private void RefreshReward()
    {

        if (chooseID != 0)
        {
            for (int i = 0; i < UITaskReward.childCount; i++)
            {
                Destroy(UITaskReward.GetChild(i).gameObject);
            }

            TaskDetails taskDetail = currentTask.TaskDetailsList.Find(i => i.taskID == chooseID);
            for (int i = 0; i < taskDetail.remuneration.Count; i++)
            {   
                ItemInfo_SO itemsInfo = taskDetail.remuneration[i];
                GameObject RewardCell = Instantiate(RewardPreviewPrefab, UITaskReward);
                RewardCell.GetComponent<TaskReward>().Refresh(itemsInfo, itemsInfo.num);
            }
        }
    }

    private void InitClik()
    {   
        UICancelButton.GetComponent<Button>().onClick.AddListener(OnClickCancel);
        UITaskTrailButton.GetComponent<Button>().onClick.AddListener(OnClickTrail);
    }

    private void OnClickCancel()
    {
        ClosePanel(UIConst.TasksPanel);
        print("----- OnClickClose");
    }

    private void OnClickTrail()
    {   
        print(chooseID);
        CurrentID = chooseID;
        playerMainUI.RefreshTaskInfo();
        print("--------" + CurrentID);
    }

    // 将总表的任务添加，移除到当前列表并标记.
    // public void AddTask(int ID)
    // {
    //     TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
    //     currentTask.TaskDetailsList.Add(taskDetails);
    // }
    // public void RemoveTask(int ID)
    // {
    //     TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
    //     currentTask.TaskDetailsList.Remove(taskDetails);
    // }
    
    // 根据当前列表的内容刷新任务UI的信息.
    
    // 选取一个当前列表选取一个任务进行跟踪.
    
    // 为相应任务对象添加触发事件
    public void ActiveCurrentTask()
    {

    }

    
    public static void CheckTaskType(TaskDetails taskDetail, ref string textContent)
    {
        if (taskDetail.taskType == TaskType.击杀)
        {
            for (int i = 0; i < taskDetail.DefaultList.Count; i++)
            {
                Default defaultList = taskDetail.DefaultList[i];
                textContent += $"{defaultList.targetData.CharacterName}: {defaultList.CurrentKill} / {defaultList.killTarget} \n";
            }
        }
        else if (taskDetail.taskType == TaskType.收集)
        {   
            for (int i = 0; i < taskDetail.CollectList.Count; i++)
            {
                Collect collect = taskDetail.CollectList[i];
                textContent += $"{collect.ItemInfo.itemName}: {collect.CollectTarget} \n";
            }
        }
        else if (taskDetail.taskType == TaskType.歼灭)
        {
        }
        else if (taskDetail.taskType == TaskType.存活)
        {
        }
    }
    
    public static void IncreaseTaskProgress(TaskDetails taskDetail,int targetId ,ref string textContent)
    {
        if (taskDetail.taskType == TaskType.击杀)
        {
            for (int i = 0; i < taskDetail.DefaultList.Count; i++)
            {
                Default defaultList = taskDetail.DefaultList[i];
                if (targetId == defaultList.targetData.Id && defaultList.CurrentKill < defaultList.killTarget)
                {
                    defaultList.CurrentKill += 1;
                }
                textContent += $"{defaultList.targetData.CharacterName}: {defaultList.CurrentKill} / {defaultList.killTarget} \n";
            }
        }
        else if (taskDetail.taskType == TaskType.收集)
        {   
            for (int i = 0; i < taskDetail.CollectList.Count; i++)
            {
                Collect collect = taskDetail.CollectList[i];
                if (targetId == collect.ItemInfo.id && collect.CurrentNumber < collect.CollectTarget)
                {
                    collect.CurrentNumber += 1;
                }
                textContent += $"{collect.ItemInfo.itemName}: {collect.CurrentNumber} / {collect.CollectTarget} \n";
            }
        }
        else if (taskDetail.taskType == TaskType.歼灭)
        {
        }
        else if (taskDetail.taskType == TaskType.存活)
        {
        }
    }
            // if (taskDetail.taskType == TaskType.击杀)
            // {
            //     for (int i = 0; i < taskDetail.DefaultLsit.Count; i++)
            //     {
            //         Default defaultList = taskDetail.DefaultLsit[i];
            //         textContent += $"{defaultList.targerData.CharacterName}: {defaultList.CurrentKill} / {defaultList.killTarget} \n";
            //     }
            //     UITaskDemand.GetComponent<Text>().text = textContent;
            // }
            // else if (taskDetail.taskType == TaskType.收集)
            // {   
            //     for (int i = 0; i < taskDetail.CollectLsit.Count; i++)
            //     {
            //         Collect collect = taskDetail.CollectLsit[i];
            //         textContent += $"{collect.ItemInfo.itemName}: {collect.CurrentNumber} / {collect.CollectTarget} \n";
            //     }
            //     UITaskDemand.GetComponent<Text>().text = textContent;
            // }
            // else if (taskDetail.taskType == TaskType.歼灭)
            // {
            //     UITaskDemand.GetComponent<Text>().text = taskDetail.ExterminateList.ToString();
            // }
            // else if (taskDetail.taskType == TaskType.存活)
            // {
            //     UITaskDemand.GetComponent<Text>().text = taskDetail.SurvivalLsit.ToString();
            // }
}
