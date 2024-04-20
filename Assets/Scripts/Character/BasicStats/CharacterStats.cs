using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Action<int, int> UpdateHealthBarOnAttack;
    // 通用人物状态
    public CharacterData_SO characterData;
    public CharacterData_SO templateData;
    int Damage = 15;

    public BuffManager buffManager;

    void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
        buffManager = GetComponent<BuffManager>();

    }

    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    
    public void TakeDamage(CharacterStats attacker, CharacterStats defender)
    {
        
    }
    // 函数重载
    public void TakeDamage(CharacterStats defender, BuffType buffType = BuffType.Normal , float buildAmount = 0)
    {
        int currentDamage = Math.Max(Damage - defender.BaseDefence, 0);
        CurrentHealth = Math.Max(CurrentHealth - currentDamage, 0);
        if (CurrentHealth <= 0)
        {
            GameManager.Instance.IncreaseTestTaskProgress(templateData.Id);
        }
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (GetComponent<BuffManager>() != null && buffType != BuffType.Normal )
        {
            GetComponent<BuffManager>().OnBuffTriggerBuildup(buffType, buildAmount);
        }
    }

    [Serializable]
    public class DamageData
    {
        public int damage;
        public BuffType buffType;
        public float buildAmount;

        public DamageData(int damage, BuffType buffType, float buildAmount)
        {
            this.damage = damage;
            this.buffType = buffType;  
            this.buildAmount = buildAmount;
        }
    }
    
}