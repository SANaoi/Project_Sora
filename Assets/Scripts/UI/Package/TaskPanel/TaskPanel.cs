using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using aoi;
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

    public GameObject TaskCellPrefab;
    public GameObject RewardPreviewPrefab;
    
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
        UITaskListContent = UITasksList.GetComponent<ScrollRect>().content;
    }

    private void RefreshUI()
    {
        RefreshTaskList();
        RefreshDetail();
        RefreshReward();
    }

    private void RefreshDetail()
    {
        print("RefreshDetail");
        if (chooseID != 0)
        {   
            TaskDetails taskDetail = currentTask.TaskDetailsList.Find(i => i.taskID == chooseID);
            UITaskName.GetComponent<Text>().text = taskDetail.taskName;
            UITaskDescription.GetComponent<Text>().text = taskDetail.taskDescription;
            if (taskDetail.taskType == TaskType.击杀)
            {
                string textContent = "";
                for (int i = 0; i < taskDetail.DefaultLsit.Count; i++)
                {
                    Default collect = taskDetail.DefaultLsit[i];
                    textContent += $"{collect.enemyPrefab.name}: {collect.CurrentKill} / {collect.killTarget} \n";
                }
                UITaskDemand.GetComponent<Text>().text = textContent;
                return;
            }
            else if (taskDetail.taskType == TaskType.收集)
            {   
                string textContent = "";
                for (int i = 0; i < taskDetail.CollectLsit.Count; i++)
                {
                    Collect collect = taskDetail.CollectLsit[i];
                    textContent += $"{collect.ItemInfo.itemName}: {collect.CurrentNumber} / {collect.CollectTarget} \n";
                }
                UITaskDemand.GetComponent<Text>().text = textContent;
                return;
            }
            else if (taskDetail.taskType == TaskType.歼灭)
            {
                UITaskDemand.GetComponent<Text>().text = taskDetail.ExterminateList.ToString();
                return;
            }
            else if (taskDetail.taskType == TaskType.存活)
            {
                UITaskDemand.GetComponent<Text>().text = taskDetail.SurvivalLsit.ToString();
                return;
            }
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
    }

    private void OnClickCancel()
    {
        ClosePanel(UIConst.TasksPanel);
        print("----- OnClickClose");
    }

    // 将总表的任务添加，移除到当前列表并标记.
    public void testAddTask(int ID)
    {
        TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
        currentTask.TaskDetailsList.Add(taskDetails);
    }
    public void testRemoveTask(int ID)
    {
        TaskDetails taskDetails = taskData.TaskDetailsList.Find(i => i.taskID == ID);
        currentTask.TaskDetailsList.Remove(taskDetails);
    }
    
    // 根据当前列表的内容刷新任务UI的信息.
    
    // 选取一个当前列表选取一个任务进行跟踪.
    
    // 为相应任务对象添加触发事件
    
}
