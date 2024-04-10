using System.Collections;
using System.Collections.Generic;
using aoi;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMainUI : BasePanel
{   

    private Transform HealthInfo;
    private Transform HealthBarMask;
    private Transform UITaskButton;
    private Transform UITaskInfo;
    private CharacterStats currentStats;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = GameManager.Instance.playerManager;
        InitInfo();
        UpdatePlayerHealthInfo(currentStats.CurrentHealth, currentStats.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitInfo()
    {
        currentStats = playerManager.GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdatePlayerHealthInfo;
        InitUI();
        InitClik();
        InitUIInfo();
    }

    void InitUI()
    {
        HealthInfo = transform.Find("LeftUP/Text");
        HealthBarMask = transform.Find("LeftUP/HealthBar/BarMask");
        UITaskButton = transform.Find("LeftUP/Task/TaskButton");
        UITaskInfo = transform.Find("LeftUP/Task/TaskInfo");
    }

    void InitClik()
    {
        UITaskButton.GetComponent<Button>().onClick.AddListener(OnClickTask);
    }

    private void InitUIInfo()
    {
        
    }

    public void UpdatePlayerHealthInfo(int currentHealth, int MaxHealth)
    {
        HealthInfo.GetComponent<Text>().text = $"{currentHealth} / {MaxHealth}";
        float sliderPercent = (float)currentHealth / MaxHealth;
        HealthBarMask.GetComponent<Image>().fillAmount = sliderPercent;
    }

    private void OnClickTask()
    {
        UIManager.Instance.OpenPanel(UIConst.TasksPanel);
    }

    public void RefreshTaskInfo()
    {
        int currentID = TaskPanel.CurrentID;
        if (currentID != 0)
        {
            string textContent = "";
            TaskDetails taskDetail = GameManager.Instance.currentTask.TaskDetailsList.Find(i => i.taskID == currentID);
            TaskPanel.CheckTaskType(taskDetail, ref textContent);
            UITaskInfo.GetComponent<Text>().text = textContent;
        }
        else
        {
            UITaskInfo.GetComponent<Text>().text = "";
        }
    }
}
