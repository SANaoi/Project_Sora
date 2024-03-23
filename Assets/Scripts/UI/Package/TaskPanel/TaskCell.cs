using System.Collections;
using System.Collections.Generic;
using aoi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskCell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private int m_taskID;
    private Transform TaskName;
    private TaskPanel m_uiParent;
    private TaskDetails m_taskDetail;

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
    
    public void Refresh(string name, int taskID, TaskPanel uiParent)
    {
        TaskName.GetComponent<Text>().text = name;
        m_uiParent = uiParent;
        m_taskID = taskID;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // print("OnPointerEnter: " + eventData.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.m_uiParent.chooseID == this.m_taskID)
            return;
        this.m_uiParent.chooseID = m_taskID;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
