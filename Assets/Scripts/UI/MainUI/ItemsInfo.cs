using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInfo : MonoBehaviour
{
    public Transform UIScrollView;
    void Start()
    {
        InitUI();
        InitScrollView();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void InitUI()
    {
        InitUIName();
    }
    private void InitUIName()
    {
        UIScrollView = transform.Find("Scroll View");
    }

    private void InitScrollView()
    {
        if (UIScrollView != null)
        {
            RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;

            for (int i = 0; i < scrollContent.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);
            }
        }
    }

    public void RefreshScroll()
    {
        print("物品刷新了");
    }
}
