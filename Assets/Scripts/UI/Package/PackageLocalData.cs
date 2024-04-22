using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalData
{
    public static PackageLocalData _instance;
    public List<PackageLocalItem> items;

    public static PackageLocalData Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new PackageLocalData();
            }
            return _instance;
        }
    }
    public void SavePackage()
    {
        // 更新UI信息
        // GameManager.Instance.RefreshUI();

        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    public List<PackageLocalItem> LoadPackage()
    {
        if (items != null)
        {
            return items;
        }
        if (PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
            items = packageLocalData.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        } 
    }

    public void AddPackageLocalItem(ItemInfo_SO itemInfo)
    {
        items = LoadPackage();
        PackageLocalItem packageLocalItem = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = itemInfo.id,
            num = itemInfo.num,
            effect = itemInfo.effect,
        };
        items.Add(packageLocalItem);
        SavePackage();
    }

    public void AddPackageLocalItem(ItemCell itemInfo)
    {
        items = LoadPackage();
        PackageLocalItem packageLocalItem = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = itemInfo.id,
            num = itemInfo.num,
            effect = itemInfo.itemInfo.effect,
        };
        items.Add(packageLocalItem);
        SavePackage();
    }
    
    public PackageLocalItem GetRemovePackageTableItemByID(int id)
    {
        items = LoadPackage();
        PackageLocalItem packageLocalItem = items.Find(i => i.id == id);
        if (packageLocalItem != null)
        {
            return packageLocalItem;
        }
        return null;
    }
    public List<PackageLocalItem> GetListWithHaveSameID(int ID)
    {
        items = LoadPackage();
        List<PackageLocalItem> list = new();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == ID)
            {
                list.Add(items[i]);
            }
        }
        return list;
    }
}

[System.Serializable]
public class PackageLocalItem
{
    public string uid;
    public int id;
    public int num;
    public List<Effect> effect;
    public override string ToString()
    {
        return $"uid: {uid}, id: {id}, num: {num}";
    }
}
