using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCell : MonoBehaviour
{
    public ItemInfo_SO itemInfo;
    public string uid;
    public int id; // 101为不可拾取的其他特殊图标
    public int type;
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
        this.type = GameManager.Instance.GetPackageTableItemById(id).type;
    }

    public void Destroy()
    {
        GameManager.Instance.IncreaseTestTaskProgress(itemInfo.id);

        GameManager.Instance.getSceneItems.gameObjectList.Remove(gameObject);
        UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().DelScrollContent(gameObject);
        Destroy(gameObject);
    }
}
