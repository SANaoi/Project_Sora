using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using aoi;


public class GMCmd
{
    [MenuItem("GMCmd/背包功能/读取背包数据")]
    public static void ReadPackageTable()
    {
        PackageTable_SO packageTable = Resources.Load<PackageTable_SO>("PackageData/PackageTable");
        if (packageTable != null)
        {
            foreach (PackageTableItem packageTableItem in packageTable.DataList)
            {
                Debug.Log(string.Format("[id]:{0}, [name]:{1}", packageTableItem.id, packageTableItem.name));
            }
        }
    }

    [MenuItem("GMCmd/背包功能/创建背包测试数据")]
    public static void CreateLocalPackageData()
    {
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
        for (int i = 1; i < 12; i++)
        {
            PackageLocalItem packageLocalItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = 2,
                num = i
            };
            PackageLocalData.Instance.items.Add(packageLocalItem);
        }
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/背包功能/清除背包数据")]
    public static void DeleteLocalPackageData()
    {
        PackageLocalData.Instance.items.Clear();
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/背包功能/删除指定背包道具")]
    public static void DeleteLocalPackageDatabByID()
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        PackageLocalItem packageLocalItem = PackageLocalData.Instance.items.Find(i => i.id == 2);
        Debug.Log(packageLocalItem.ToString());
        if (packageLocalItem != null)
        {
            items.Remove(packageLocalItem);
        }
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/背包功能/读取背包测试数据")]
    public static void ReadLoadPackData()
    {
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readItems)
        {
            Debug.Log(item);
        } 
    }

    [MenuItem("GMCmd/背包功能/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }

    [MenuItem("GMCmd/选中物品的SelectingID")]
    public static void PrintInfo()
    {
        Debug.Log($"当前选中物品的SelectingID: {PlayerManager.Instance.SelectingID}");
    }

    [MenuItem("GMCmd/输入系统/激活角色输入")]
    public static void ActivePlayerInputSystem()
    {
        EventCenter.Instance.EventTrigger(EventConst.ActiveInputSystem);
    }

    [MenuItem("GMCmd/输入系统/注销角色输入")]
    public static void LogoutPlayerInputSystem()
    {
        EventCenter.Instance.EventTrigger(EventConst.LogoutInputSystem);
    }

    [MenuItem("GMCmd/对话系统/打开测试对话框")]
    public static void OpenTestDialog()
    {
        UIManager.Instance.OpenPanel(UIConst.DialogBox);
    }

    [MenuItem("GMCmd/任务系统/打开任务列表")]
    public static void OpenTasksPanel()
    {
        TaskPanel taskPanel = UIManager.Instance.OpenPanel(UIConst.TasksPanel).GetComponent<TaskPanel>();
        Debug.Log(TaskPanel.CurrentID);
        
    }

    // [MenuItem("GMCmd/任务系统/添加测试任务")]
    // public static void AddDefaultTask()
    // {
    //     BasePanel taskPanel = UIManager.Instance.OpenPanel(UIConst.TasksPanel) as TaskPanel;
    //     taskPanel.GetComponent<TaskPanel>().AddTask(1);
    // }
    // [MenuItem("GMCmd/任务系统/移除测试任务")]
    // public static void ReMoveDefaultTask()
    // {
    //     BasePanel taskPanel = UIManager.Instance.OpenPanel(UIConst.TasksPanel) as TaskPanel;
    //     taskPanel.GetComponent<TaskPanel>().RemoveTask(1);
    // }
    [MenuItem("GMCmd/任务系统/增加测试任务进度")]
    public static void IncreaseTestTaskProgress()
    {
        string textContent = "";
        int currentID = TaskPanel.CurrentID;
        aoi.TaskDetails taskDetail = GameManager.Instance.currentTask.TaskDetailsList.Find(i => i.taskID == currentID);
        if (taskDetail != null)
        {
            TaskPanel.IncreaseTaskProgress(taskDetail, 1, ref textContent);
            (UIManager.Instance.OpenPanel(UIConst.PlayerMainUI) as PlayerMainUI).RefreshTaskInfo();
        }
    }
    [MenuItem("GMCmd/任务系统/交付任务")]
    public static void PostTask()
    {
        aoi.TaskDetails taskDetail = GameManager.Instance.currentTask.TaskDetailsList.Find(i => i.taskID == 1);
        if (taskDetail != null)
        {
            GameManager.Instance.IsCompleteTask(taskDetail);
            GameManager.Instance.GetTaskReward(taskDetail);
            GameManager.Instance.ReMoveCurrentTask(taskDetail);
            (UIManager.Instance.OpenPanel(UIConst.PlayerMainUI) as PlayerMainUI).RefreshTaskInfo();
            
            Debug.Log("完成任务");
        }
    }

}
