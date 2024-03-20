using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData_SO", menuName = "Data/Test4/CurrentTask")]
public class CurrentTask_SO : ScriptableObject
{
    [Header("任务数据")]
    public string taskID;
    public string taskName;
    public TaskType taskType;
    [TextArea]
    public string taskDescription;//任务简介

    [Header("任务目标")]


    [Header("敌人数量与类型")]
    public List<Wave> waves;

    [System.Serializable]
    public class Wave//（RPG模式下不需要）
    {
        public string waveName;
        [Header("敌人类型")]
        public List<EnemyGroup> enemyGroups;
        [Tooltip("在波次中生成的敌人总数，不能为0")] public int waveQuota;
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
    public bool onTask;//是否启动
    public bool taskCompleted;//是否完成
    public bool isMandatoryTask;//是否为强制任务

    /// <summary>
    /// 清空列表内容
    /// </summary>
    public void ResetTaskData()
    {
        taskID = string.Empty;
        taskName = string.Empty;
        taskType = TaskType.Default; // 你需要根据实际情况设置默认值
        taskDescription = string.Empty;

        // 清空波次和敌人组
        waves.Clear();

        // 重新初始化参数字段
        currentWaveCount = 0;
        waveInterval = 0f;
        enemiesAlive = 0;
        maxEnemiesAllowed = 0;
        maxEnemiesReached = false;
        isWaveActive = false;

        // 清空任务报酬和状态字段
        remuneration = 0;
        onTask = false;
        taskCompleted = false;
        isMandatoryTask = false;
    }

}
