using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    private Transform UIIcon;
    private Transform UISelect;
    private Transform UIDeleteSelect;
    private Transform UIName;
    private Animator UIMouseOverAni;
    private Animator UIMouseSelectedAni;

    private PackageLocalItem packageLocalItem;
    private PackageTableItem packageTableItem;
    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/Icon");
        UIName = transform.Find("Bottom/Name");
        UISelect = transform.Find("Select");
        UIDeleteSelect = transform.Find("DeleteSelect");
        UIMouseOverAni = transform.Find("MouseOverAni").GetComponent<Animator>();
        UIMouseSelectedAni = transform.Find("SelectAni").GetComponent<Animator>();
    }

    public void Refresh(PackageLocalItem packageLocalItem, PackagePanel uiParent)
    {
       this.packageLocalItem = packageLocalItem;
       this.uiParent = uiParent;
       this.packageTableItem = GameManager.Instance.GetPackageTableItemById(packageLocalItem.id);

       Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.imagePath);
       Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
       UIIcon.GetComponent<Image>().sprite = temp;

       UIName.GetComponent<Text>().text = packageTableItem.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnPointerEnter: " + eventData.ToString());
        UIMouseOverAni.SetTrigger("In");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("OnPointerClick: " + eventData.ToString());
        UIMouseSelectedAni.SetTrigger("In");
        if (this.uiParent.chooseUID == this.packageLocalItem.uid)
            return;
        this.uiParent.chooseUID = this.packageLocalItem.uid;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("OnPointerExit: " + eventData.ToString());
        UIMouseOverAni.SetTrigger("Out");

    }
}
