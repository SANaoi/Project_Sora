using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMainUI : BasePanel
{   

    private Transform HealthInfo;
    private Transform HealthBarMask;
    private CharacterStats currentStats;
    void Start()
    {
        InitInfo();
        UpdatePlayerHealthInfo(currentStats.CurrentHealth, currentStats.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitInfo()
    {
        currentStats = PlayerManager.Instance.GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdatePlayerHealthInfo;
        InitUI();
        InitUIInfo();
    }

    void InitUI()
    {
        HealthInfo = transform.Find("LeftUP/Text");
        HealthBarMask = transform.Find("LeftUP/HealthBar/BarMask");
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
}
