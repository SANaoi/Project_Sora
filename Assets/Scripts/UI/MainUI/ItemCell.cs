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
    public string m_name;
    [HideInInspector]
    public string itemName;
    public bool isShowNum;
    public int num;
    public string imagePath;

    void Start()
    {
        this.uid = Guid.NewGuid().ToString();
        this.id = itemInfo.id;
        
        if (m_name == "")
        {
            this.itemName = itemInfo.itemName;
        }
        else
        {
            this.itemName = m_name;
        }
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
