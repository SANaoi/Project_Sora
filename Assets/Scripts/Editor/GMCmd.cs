using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class GMCmd
{
    [MenuItem("GMCmd/读取背包数据")]
    public static void ReadPackageTable()
    {
        PackageTable_SO packageTable = Resources.Load<PackageTable_SO>("PackageData/PackageTable");
        if (packageTable != null)
        {
            foreach (PackageTableItem packageTableItem in packageTable.DataList)
            {
                Debug.Log(string.Format("[id]:{0}, [name]:{1}", packageTableItem.id, packageTableItem.name));
            }
        }
    }

    [MenuItem("GMCmd/创建背包测试数据")]
    public static void CreateLocalPackageData()
    {
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
        for (int i = 1; i < 12; i++)
        {
            PackageLocalItem packageLocalItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = 2,
                num = i
            };
            PackageLocalData.Instance.items.Add(packageLocalItem);
        }
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/清除背包数据")]
    public static void DeleteLocalPackageData()
    {
        PackageLocalData.Instance.items.Clear();
        PackageLocalData.Instance.SavePackage();
    }

    [MenuItem("GMCmd/读取背包测试数据")]
    public static void ReadLoadPackData()
    {
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readItems)
        {
            Debug.Log(item);
        } 
    }

    [MenuItem("GMCmd/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }

    [MenuItem("GMCmd/选中物品的SelectingID")]
    public static void PrintInfo()
    {
        Debug.Log($"当前选中物品的SelectingID: {PlayerManager.Instance.SelectingID}");
    }
}
