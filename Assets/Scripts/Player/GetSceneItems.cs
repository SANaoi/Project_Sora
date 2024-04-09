using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetSceneItems : MonoBehaviour
{    
    
    // 场景可获取物品功能参数
    public List<GameObject> gameObjectList;
    [HideInInspector]
    public string SelectingID;
    
    
    private float searchRadius;

    private void Start()
    {
        searchRadius = 2.0f;
        SelectingID = "";
        gameObjectList = new();

        InvokeRepeating("SearchForGameObjectWithTag", 0.25f, 0.25f);
    }

    #region 检索周围可拾取物品
    private void SearchForGameObjectWithTag()
    {   
        var foundObjects = Physics.OverlapSphere(transform.position, searchRadius);
        
        List<GameObject> foundItems = new();
        List<GameObject> ToBeDeleted  = new();
        ItemsInfo itemsInfo = UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>();
        foreach (var foundObject in foundObjects)
        {
            if (foundObject.CompareTag("Item") || foundObject.CompareTag("NPC"))
            {
                if (foundObject.GetComponent<ItemCell>() == null)
                {
                    return;
                }
                foundItems.Add(foundObject.gameObject);
                string NowUid = foundObject.GetComponent<ItemCell>().uid;
                if (!gameObjectList.Any(gameObject => gameObject.GetComponent<ItemCell>().uid == NowUid))
                {
                    gameObjectList.Add(foundObject.gameObject);
                    itemsInfo.GetScrollContent(foundObject.gameObject);
                }
            }
        }
        
        foreach (GameObject foundObject in gameObjectList)
        {   
            string uid = foundObject.GetComponent<ItemCell>().uid;
            if (!foundItems.Any(gameObject => gameObject.GetComponent<ItemCell>().uid == uid))
            {
                ToBeDeleted.Add(foundObject.gameObject);
            }
        }
        foreach (GameObject obj in ToBeDeleted)
        {
            gameObjectList.Remove(obj);
            itemsInfo.DelScrollContent(obj);
        }

        // 刷新选中的物品
        if (gameObjectList.Count == 0)
        {
            SelectingID = "";
            return;
        }
        if (SelectingID != "")
        {
            for (int i = 0; i < itemsInfo.scrollContent.childCount; i++)
            {
                ItemDetail temp = itemsInfo.scrollContent.GetChild(i).GetComponent<ItemDetail>();
                if (temp.UISelecting.gameObject.activeSelf == true)
                {
                    return;
                }
            }
        }
        if (itemsInfo.scrollContent.childCount == 0)
        {
            SelectingID = "";
            return;
        }
        SelectingID = itemsInfo.scrollContent.GetChild(0).GetComponent<ItemDetail>().uid;
        itemsInfo.scrollContent.GetChild(0).GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);

    }
    #endregion
}
