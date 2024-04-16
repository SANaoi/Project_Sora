using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    public Transform UISelecting;
    private Transform UIName;
    private Transform UINum;
    private Transform UIImage;

    public string uid;
    void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        InitUIName();
        UISelecting.gameObject.SetActive(false);
    }

    private void InitUIName()
    {
        UISelecting = transform.Find("Selecting");
        UIName = transform.Find("name");
        UIImage = transform.Find("Image");
        UINum = transform.Find("num");

    }

    public void RefreshUI(ItemCell ItemcellInfo)
    {
        uid = ItemcellInfo.uid;

        UIName.GetComponent<Text>().text = ItemcellInfo.itemName;

        Texture2D t = (Texture2D)Resources.Load(ItemcellInfo.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIImage.GetComponent<Image>().sprite = temp;
        if (ItemcellInfo.isShowNum)
        {
            UINum.GetComponent<Text>().text = ItemcellInfo.num.ToString();
        }
        else
        {
            UINum.GetComponent<Text>().text = "";
        }
    }
}
