using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageDetail : MonoBehaviour
{
    private Transform UITitle;
    private Transform UIIcon;
    private Transform UIName;
    private Transform UIDescription;

    private PackageLocalItem packageLocalData;
    private PackageTableItem packageTableItem;
    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UITitle = transform.Find("Top/Title");
        UIIcon = transform.Find("Center/Image");
        UIName = transform.Find("Center/Description");
        UIDescription = transform.Find("Bottom/Description");
    }

    public void Refresh(PackageLocalItem packageLocalData, PackagePanel uiParent)
    {
        // 初始化
        this.uiParent = uiParent;
        this.packageTableItem = GameManager.Instance.GetPackageTableItemById(packageLocalData.id);
        this.packageLocalData = packageLocalData;

        UITitle.GetComponent<Text>().name = this.packageTableItem.name;
        UIDescription.GetComponent<Text>().text = this.packageTableItem.description;

        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        UIName.GetComponent<Text>().text = this.packageTableItem.name;
        UIDescription.GetComponent<Text>().text = this.packageTableItem.description;
    }
}
