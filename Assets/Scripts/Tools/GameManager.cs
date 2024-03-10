using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private PackageTable_SO packageTable;

    public ShootController shootController;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerMoveControls.Instance.Player.OpenPackage.performed += GetOpenPackageInput;

        // shootController = new ShootController();
    }
    
    #region 背包的基础功能
    public PackageTable_SO GetPackageTable()
    {
        if(packageTable == null)
        {
            packageTable = Resources.Load<PackageTable_SO>("PackageData/PackageTable");
        }
        return packageTable;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }

    public PackageTableItem GetPackageTableItemById(int id)
    {
        List<PackageTableItem> packageDataList = GetPackageTable().DataList;
        foreach(PackageTableItem item in packageDataList)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    public PackageLocalItem GetPackageTableItemByUId(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach(PackageLocalItem item in packageDataList)
        {
            if(item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }

    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }

    public void GetOpenPackageInput(InputAction.CallbackContext context)
    {
        if(!UIManager.Instance.OpenPanel(UIConst.PackagePanel))
        {
            UIManager.Instance.OpenPanel(UIConst.PackagePanel);
        }
    }
    
    public int GetPackageLocalItemsNumById(int id)
    {
        List<PackageLocalItem> items = PackageLocalData.Instance.LoadPackage();
        int res = 0;
        foreach (PackageLocalItem item in items)
        {
            if (item.id == id)
            {
                res += item.num;
            }
        }
        return res;
    }
    #endregion

    #region 界面UI基础功能


    #endregion
}

public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageTableItem x = GameManager.Instance.GetPackageTableItemById(a.id);
        PackageTableItem y = GameManager.Instance.GetPackageTableItemById(b.id);
        int idComparison = y.id.CompareTo(x.id);
        if (idComparison == 0)
        {
            return b.num.CompareTo(a.num);
        }
        return idComparison;
    }
}
