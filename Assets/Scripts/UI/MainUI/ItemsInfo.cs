using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInfo : MonoBehaviour
{
    private Transform UIScrollView;
    public RectTransform scrollContent;
    public GameObject ItemCellPrefab;
    void Start()
    {
        InitUI();
        InitScrollView();
    }

    private void InitUI()
    {
        InitUIName();
    }
    private void InitUIName()
    {
        UIScrollView = transform.Find("Scroll View");
        scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
    }

    private void InitScrollView()
    {
        if (UIScrollView != null)
        {
            for (int i = 0; i < scrollContent.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);
            }
        }
    }

    public void GetScrollContent(GameObject gameObject)
    {
        ItemCell ItemcellInfo = gameObject.GetComponent<ItemCell>();
        
        Transform Item = Instantiate(ItemCellPrefab.transform, scrollContent) as Transform;
        Item.GetComponent<ItemDetail>().RefreshUI(ItemcellInfo);
    }

    public void DelScrollContent(GameObject gameObject)
    {
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            string temp = scrollContent.GetChild(i).GetComponent<ItemDetail>().uid;
            if (gameObject.GetComponent<ItemCell>().uid == temp)
            {   
                Destroy(scrollContent.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void UpSelectID()
    {
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (PlayerManager.Instance.SelectingID == scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().uid)
            {
                if (i > 0)
                {
                    PlayerManager.Instance.SelectingID = scrollContent.GetChild(i - 1).gameObject.GetComponent<ItemDetail>().uid;
                    scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(false);
                    scrollContent.GetChild(i - 1).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void DownSelectID()
    {
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (PlayerManager.Instance.SelectingID == scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().uid)
            {
                if (i < scrollContent.childCount - 1)
                {
                    PlayerManager.Instance.SelectingID = scrollContent.GetChild(i + 1).gameObject.GetComponent<ItemDetail>().uid;
                    scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(false);
                    scrollContent.GetChild(i + 1).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);
                    break;
                }
            }
        }

    }
}
