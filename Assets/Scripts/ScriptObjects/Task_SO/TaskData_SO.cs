using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aoi
{

[CreateAssetMenu(fileName = "TaskData_SO", menuName = "Data/Task/Task")]
public class TaskData_SO : ScriptableObject
{
    public List<TaskDetails> TaskDetailsList;
}

[System.Serializable]
public class TaskDetails
{

    [Header("任务数据")]
    [Header("ID与名称")]
    public int taskID; // 任务ID
    public string taskName; // 任务名称
    [TextArea]
    public string taskDescription;//任务简介
    public TaskType taskType;// 任务类型
    [Header("击杀")] public List<Default> DefaultList;
    [Header("歼灭")] public List<Exterminate> ExterminateList;
    [Header("收集")] public List<Collect> CollectList;
    [Header("存活")] public List<Survival> SurvivalList;

    [Header("参数")]
    [Header("任务报酬")] public List<ItemInfo_SO> remuneration;
    [Header("任务是否完成(是否允许重复出现)")] public bool taskCompleted;
    [Header("是否是强制任务")] public bool isMandatoryTask;

    

}

# region 任务类型
[System.Serializable]
public class Default
{
    public CharacterData_SO targetData; 
    public int CurrentKill;
    public int killTarget;
}
[System.Serializable]
public class Exterminate
{
    public CharacterData_SO targerData; 
    public int killTarget;
}
[System.Serializable]
public class Collect
{
    public ItemInfo_SO ItemInfo;
    public int CurrentNumber;
    public int CollectTarget;
}
[System.Serializable]
public class Survival
{
    public int SurvivalTime;
}
# endregion

}


    // [Tooltip("当前波数,默认为0")] public int currentWaveCount;
    // [Tooltip("波次间隔")] public float waveInterval;
    // [Tooltip("敌人存活数")] public int enemiesAlive;
    // [Tooltip("允许的最大敌人数")] public int maxEnemiesAllowed;
    // [Tooltip("是否达到最大敌人数")] public bool maxEnemiesReached;
    // [Tooltip("是否启动出怪")] public bool isWaveActive = false;
    

    // [TextArea]
    // public string taskDescription;//任务简介
    // [Header("任务目标")]
    // [Header("敌人数量与类型")]
    
    // public List<Wave> waves;

    
    // void Refresh()
    // {
    //     switch (taskType)
    //     {
    //         case TaskType.collect:
    //         Debug.Log("Collect");
    //         break;
    //     }
    // }

    // # region 歼灭
    // [System.Serializable]
    // public class Wave
    // {
    //     public string waveName;
    //     [Header("敌人类型")]
    //     public List<EnemyGroups> enemyGroups;
    //     [Tooltip("波次中敌人生成数")] public int waveQuota;
    //     [Tooltip("生成间隔")] public float spawnInterval;
    //     [Tooltip("已生成敌人数,默认为0")] public int spawnCount;
    // }

    // [System.Serializable]
    // public class EnemyGroups
    // {
    //     public int enemyID;
    //     public int enemyName;
    //     [Tooltip("敌人数")] public int enemyCount;
    //     [Tooltip("波数")] public int spawnCount;   
    //     public GameObject enemyPrefab; 
    // }
    // # endregion


    // # region 收集
    // void ShowCollect()
    // {
        
    // }
    // [System.Serializable]
    // public class Collect
    // {
    //     public string CollectName;
    //     public List<Dictionary<ItemInfo_SO, int>> CollectObjects;
    // }
    // # endregion
