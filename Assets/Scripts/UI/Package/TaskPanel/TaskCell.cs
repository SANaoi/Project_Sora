using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCell : MonoBehaviour
{
    private int TaskID;
    private Transform TaskName;

    // 任务数据表

    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        TaskName = transform.Find("TaskName");
    }
    
}
