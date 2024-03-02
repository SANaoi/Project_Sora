using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCell : MonoBehaviour
{
    public ItemInfo_SO itemInfo;
    public string uid;
    public int id;
    public string itemName;
    public int num;

    void Start()
    {
        this.uid = Guid.NewGuid().ToString();
        this.id = itemInfo.id;
        this.itemName = itemInfo.itemName;
        this.num = itemInfo.num;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
