using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items Config", menuName = "Item/Item Info")]
public class ItemInfo_SO : ScriptableObject
{
    public int id; // 识别物品的类型
    public string itemName; // 显示名字
    public int num = 30;
}

