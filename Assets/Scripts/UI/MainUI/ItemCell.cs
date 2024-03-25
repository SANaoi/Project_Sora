using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCell : MonoBehaviour
{
    public ItemInfo_SO itemInfo;
    public string uid;
    public int id; // 101为不可拾取的其他特殊图标
    public string itemName;
    public int num;
    public string imagePath;

    void Start()
    {
        this.uid = Guid.NewGuid().ToString();
        this.id = itemInfo.id;
        this.itemName = itemInfo.itemName;
        this.num = itemInfo.num;
        this.imagePath = GameManager.Instance.GetPackageTableItemById(id).imagePath;
    }

    public void Destroy()
    {
        GameManager.Instance.IncreaseTestTaskProgress(itemInfo.id);

        PlayerManager.Instance.gameObjectList.Remove(gameObject);
        UIManager.Instance.ItemsInfo.DelScrollContent(gameObject);
        Destroy(gameObject);
    }
}
