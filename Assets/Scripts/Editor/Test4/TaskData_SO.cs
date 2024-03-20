using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData_SO", menuName = "Data/Test4/Task")]
public class TaskData_SO : ScriptableObject
{
    public List<TaskDetails> TaskDetailsList;
}


[System.Serializable]
public class TaskDetails
{
    [Header("任务数据，ID以1开头")]
    [Header("ID与头像")]
    public int taskID;//任务ID（必要）
    public string taskName;//任务名称
    public TaskType taskType;//任务类型
    [TextArea]
    public string taskDescription;//任务简介

    [Header("任务目标")]
    [Header("敌人数量与类型")]
    public List<Wave> waves;//敌人波次

    //public List<EnemyGroup> enemyGroups;//RPG类游戏情况下


    [System.Serializable]
    public class Wave//（RPG模式下不需要）
    {
        public string waveName;
        [Header("敌人类型")]
        public List<EnemyGroup> enemyGroups;
        [Tooltip("在波次中生成的敌人总数")] public int waveQuota;
        [Tooltip("生成间隔")] public float spawnInterval;//生成间隔
        [Tooltip("已生成敌人数，默认为0")] public int spawnCount;

    }

    [System.Serializable]
    public class EnemyGroup
    {
        public int enemyID;
        public string enemyName;
        [Tooltip("敌人数")] public int enemyCount;//敌人数
        [Tooltip("波数")] public int spawnCount;//生成数（RPG模式下不需要）
        public GameObject enemyPrefab;

    }

    [Header("参数")]
    #region RPG模式下不需要
    [Tooltip("当前波次，默认为0")] public int currentWaveCount;//当前波次
    [Tooltip("波次间隔")] public float waveInterval;
    [Tooltip("敌人存活数")] public int enemiesAlive;//敌人存活数
    [Tooltip("允许的最大敌人数")] public int maxEnemiesAllowed;//允许的最大敌人数
    [Tooltip("是否达到最大敌人数")] public bool maxEnemiesReached = false;//是否达到最大敌人数
    [Tooltip("是否启动出怪")] public bool isWaveActive = false;//是否启动出怪
    #endregion
    [Header("任务报酬")]
    public int remuneration;
    [Header("任务状态")]
    public bool onTask;
    [Header("任务是否完成（是否重复出现）")]
    public bool taskCompleted;
    [Header("是否是强制任务")]
    public bool isMandatoryTask;
}


