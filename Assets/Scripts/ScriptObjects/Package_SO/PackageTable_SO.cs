using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "package/packageTable", fileName = "PackageTable")]
public class PackageTable_SO : ScriptableObject
{
    public List<PackageTableItem> DataList = new List<PackageTableItem>();
}

[System.Serializable]
public class PackageTableItem
{
    public int id; // 唯一标识
    public int type;
    public string name;
    public string description;
    public string imagePath;
}
