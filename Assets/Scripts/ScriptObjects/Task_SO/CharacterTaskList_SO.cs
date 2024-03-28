using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterTaskScript_SO", menuName = "Data/Task/CharacterTaskScript")]
public class CharacterTaskList_SO : ScriptableObject
{
    public List<TaskList> textAssets;
}

[System.Serializable]
public class TaskList
{
    [Header("文本")]
    public TextAsset TaskScript;
    [Header("对应任务ID")]public int taskID;
    [Header("是否接取")]public bool IsAccepted;
    [Header("是否完成")]public bool IsCompleted;
}
