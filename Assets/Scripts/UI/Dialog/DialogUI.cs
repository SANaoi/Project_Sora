using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class DialogUI : BasePanel
{
    Transform SpeakerName;
    Transform DialogueContent;
    public Transform SelectBox;
    public TextAsset dialogDataFile;

    public bool isSelecting;
    public int dialogIndex;
    string[] dialogRows;

    
    void Awake()
    {
        InitUI();
    }

    void Start()
    {
        // TODO 测试调用，待删除
        reloadData(dialogDataFile);
        ShowDialogRows();

        GameManager.Instance.inputActions.Player.Fire.started += OnClickNext;
        SelectBox = UIManager.Instance.OpenPanel(UIConst.SelectBox).transform;
    }
    void InitUI()
    {
        SpeakerName = transform.Find("Speaker/SpeakerName");
        DialogueContent = transform.Find("Dialogue Content");
    }

    public void reloadData(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split("\n");

        // foreach (var row in rows)
        // {
        //     string[] cell = row.Split(",");
        // }
    }

// 0:标志 1:ID  2:人物	3:内容	4:跳转	5:效果	6:目标
    public void ShowDialogRows()
    {
        if (!isSelecting)
        {
            for (int i = 0; i < dialogRows.Length; i++ )
            {
                string[] cells = dialogRows[i].Split(',');
                if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
                {
                    UpdateText(cells[2], cells[3]);

                    dialogIndex = int.Parse(cells[4]);
                    break;
                }

                else if (cells[0] == "End"  && int.Parse(cells[1]) == dialogIndex)
                {
                    GameManager.Instance.inputActions.Player.Fire.started -= OnClickNext;
                    ClosePanel(UIConst.DialogBox);
                }

                else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
                {
                    SetSelectButton(dialogIndex);
                }
            }
        }
    }

    private void SetSelectButton(int id)
    {
        for (int i = id; i < dialogRows.Length; i++ )
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "&")
            {
                if (!SelectBox)
                {
                    SelectBox = UIManager.Instance.OpenPanel(UIConst.SelectBox).transform;
                }
                SelectBox.GetComponent<SelectBoxUI>().SetSelectCell(
                    cells[3], 
                    int.Parse(cells[4]), 
                    cells[5], 
                    cells[6]
                );
            }
        }
        isSelecting = true;
    }

    void UpdateText(string name, string Content)
    {
        SpeakerName.GetComponent<Text>().text = name;
        DialogueContent.GetComponent<Text>().text = Content;
    }

    void OnClickNext(InputAction.CallbackContext context)
    {
        ShowDialogRows();
    }
}
