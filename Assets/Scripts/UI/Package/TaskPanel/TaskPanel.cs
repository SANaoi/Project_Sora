using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    private Transform UICancelButton;
    private Transform UITasksList;
    private Transform UITaskName;
    private Transform UITaskDescription;
    private Transform UITaskDemand;
    private Transform UITaskReward;

    public GameObject TaskCellPrefab;
    public GameObject RewardPreview;

    private void Awake()
    {
        InitUI();
        InitClik();

    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UICancelButton = transform.Find("CenterTop/Cancel");
        UITasksList = transform.Find("Center/TasksList");
        UITaskName = transform.Find("Center/TaskDetail/Top/TaskName");
        UITaskDescription = transform.Find("Center/TaskDetail/Center/Description");
        UITaskDemand = transform.Find("Center/TaskDetail/Center/Demand");
        UITaskReward = transform.Find("Center/TaskDetail/Bottom");
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
}
