using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDX.Collections.Generic;

[CreateAssetMenu(fileName = "Items Config", menuName = "Item/Item Info")]
public class ItemInfo_SO : ScriptableObject
{
    public int id; // 识别物品的类型
    public string itemName; // 显示名字
    public int num = 30;
    
    public List<Effect> effect;
}
[System.Serializable] 
public class Effect
{
    public int effectId;
    public EffectType effectType;
    public int effectAmount;

}

public enum EffectType
{
    None, 恢复, 提速
}
