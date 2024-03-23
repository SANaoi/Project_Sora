using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskReward : MonoBehaviour
{
    private Transform m_image;
    private Transform m_number;
    
    
    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        m_image = transform.Find("Image");
        m_number = transform.Find("Number");
    }

    public void Refresh(ItemInfo_SO itemInfo, int num)
    {   
        string imagePath = GameManager.Instance.GetPackageTableItemById(itemInfo.id).imagePath;

        Texture2D t = (Texture2D)Resources.Load(imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        m_image.GetComponent<Image>().sprite = temp;

        m_number.GetComponent<Text>().text = num.ToString();

    }
}           