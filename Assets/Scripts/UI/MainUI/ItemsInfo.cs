using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInfo : BasePanel
{
    private Transform UIScrollView;
    public RectTransform scrollContent;
    public GameObject ItemCellPrefab;
    private PlayerManager playerManager;
    void Awake()
    {
        // playerManager = FindAnyObjectByType<PlayerManager>();
        InitUI();
        InitScrollView();

    }
    void Start()
    {
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
        playerManager = FindAnyObjectByType<PlayerManager>();
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (playerManager.SelectingID == scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().uid)
            {
                if (i > 0)
                {
                    playerManager.SelectingID = scrollContent.GetChild(i - 1).gameObject.GetComponent<ItemDetail>().uid;
                    scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(false);
                    scrollContent.GetChild(i - 1).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void DownSelectID()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (playerManager.SelectingID == scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().uid)
            {
                if (i < scrollContent.childCount - 1)
                {
                    playerManager.SelectingID = scrollContent.GetChild(i + 1).gameObject.GetComponent<ItemDetail>().uid;
                    scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(false);
                    scrollContent.GetChild(i + 1).gameObject.GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public int GetItemNumByUID(string uid)
    {
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (scrollContent.GetChild(i).gameObject.GetComponent<ItemDetail>().uid == uid)
            {
                return i;
            }
        }
        return -1;
    }
}
