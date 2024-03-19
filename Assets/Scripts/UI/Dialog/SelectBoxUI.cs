using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBoxUI : BasePanel
{
    public GameObject SelectCellPrefab;
    void Start()
    {
        InitUI();
    }


    public void InitUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SetSelectCell(string content, int jumpTo, string effect, string target)
    {
        Transform SelectCell = Instantiate(SelectCellPrefab.transform, this.transform);
        SelectCell.GetComponent<SelectCellUI>().InitParameter(content, jumpTo, effect, target);
        SelectCell.GetChild(0).GetComponent<Text>().text = content;
    }
}
