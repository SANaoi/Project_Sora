using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    private Transform UIMenu;
    private Transform UIMenuAll;
    private Transform UIMenuWeapon;
    private Transform UITabName;
    private Transform UICloseBtn;
    private Transform UICenter;
    private Transform UIScrollView;
    private Transform UIDetailPanel;
    private Transform UILeftBtn;
    private Transform UIRightBtn;
    private Transform UIDeletePanel;
    private Transform UIDeleteBackBtn;
    private Transform UIDeleteInfoText;
    private Transform UIDeleteConfirmBtn;
    private Transform UIBottomMenus;
    private Transform UIDeleteBtn;
    private Transform UIDetailBtn;

    public GameObject PackageUIItemPrefab;

    private string _chooseUid;
    public string chooseUID
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }

    private void Awake()
    {
        print(this.name + "  Awake----------");
    }
    private void OnEnable()
    {
        print(this.name + "  OnEnable----------");
        InitUI();
        InitClik();
    }


    private void OnDisable()
    {
        print(this.name + "  OnDisable----------");
        // RemoveListener();
    }

    private void Start()
    {
        print(this.name + "  Start----------");
        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshScroll();
    }

    private void RefreshDetail()
    {
        PackageLocalItem localItem = GameManager.Instance.GetPackageTableItemByUId(chooseUID);

        UIDetailPanel.GetComponent<PackageDetail>().Refresh(localItem, this);
    }
    private void RefreshScroll()
    {
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;

        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        foreach (PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        {
            Transform packageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = packageUIItem.GetComponent<PackageCell>();
            packageCell.Refresh(localData, this);

        }
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIMenu = transform.Find("CenterTop/Menus");
        UIMenuAll = transform.Find("CenterTop/Menus/All");
        UIMenuWeapon = transform.Find("CenterTop/Menus/Weapon");
        UITabName = transform.Find("LeftTop/TabName");
        UICloseBtn = transform.Find("RightTop/Close");
        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");
        UIDetailPanel = transform.Find("Center/DetailPanel");
        UILeftBtn = transform.Find("Left/Button");
        UIRightBtn = transform.Find("Right/Button");
        // UIDeletePanel = transform.Find("Bottom/");
        UIDeleteBackBtn = transform.Find("Bottom/DeletePanelBtn");
        // UIDeleteInfoText = transform.Find();
        // UIDeleteConfirmBtn = transform.Find();
        // UIBottomMenus = transform.Find();
        // UIDeleteBtn = transform.Find();
        // UIDetailBtn = transform.Find();
    }

    private void InitClik()
    {
        UIMenuAll.GetComponent<Button>().onClick.AddListener(OnClickAll);
        UIMenuWeapon.GetComponent<Button>().onClick.AddListener(OnClickWeapon);
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        UILeftBtn.GetComponent<Button>().onClick.AddListener(OnClickLeft);
        UIRightBtn.GetComponent<Button>().onClick.AddListener(OnClickRight);
        UIDeleteBackBtn.GetComponent<Button>().onClick.AddListener(OnClickDelete);
    }

    private void RemoveListener()
    {
        UIMenuAll.GetComponent<Button>().onClick.RemoveListener(OnClickAll);
        UIMenuWeapon.GetComponent<Button>().onClick.RemoveListener(OnClickWeapon);
        UICloseBtn.GetComponent<Button>().onClick.RemoveListener(OnClickClose);
        UILeftBtn.GetComponent<Button>().onClick.RemoveListener(OnClickLeft);
        UIRightBtn.GetComponent<Button>().onClick.RemoveListener(OnClickRight);
        UIDeleteBackBtn.GetComponent<Button>().onClick.RemoveListener(OnClickDelete);
    }

    private void OnClickAll()
    {
        print("----- OnClickAll");
    }

    private void OnClickWeapon()
    {
        print("----- OnClickWeapon");
    }

    public void OnClickClose()
    {
        print("----- OnClickClose");
        UIManager.Instance.ClosePanel("PackagePanel", true);

    }

    private void OnClickLeft()
    {
        print("----- OnClickLeft");
    }

    private void OnClickRight()
    {
        print("----- OnClickRight");
    }

    private void OnClickDelete()
    {
        print("----- OnClickDelete");
    }
}
