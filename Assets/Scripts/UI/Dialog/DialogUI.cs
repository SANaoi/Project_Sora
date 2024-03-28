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
    // 任务相关
    public TaskList taskList;
    public bool IsAccepted;
    public bool IsCompleted;

    
    void Awake()
    {
        InitUI();
    }

    void OnEnable()
    {
        print("OnEnable");
        GameManager.Instance.inputActions.Player.Fire.started += OnClickNext;
        SelectBox = UIManager.Instance.OpenPanel(UIConst.SelectBox).transform;
        EventCenter.Instance.EventTrigger(EventConst.LogoutInputSystem);
    }

    void OnDisable() 
    {
        print("OnDisable");
        GameManager.Instance.inputActions.Player.Fire.started -= OnClickNext;
        EventCenter.Instance.EventTrigger(EventConst.ActiveInputSystem);
    }

    void InitUI()
    {
        SpeakerName = transform.Find("Speaker/SpeakerName");
        DialogueContent = transform.Find("Dialogue Content");
    }

    public void reloadData(TaskList _textAsset)
    {
        taskList = _textAsset;
        dialogRows = _textAsset.TaskScript.text.Split("\n");
        IsAccepted = false;
    }

// 0:标志 1:ID  2:人物	3:内容	4:跳转	5:效果	6:目标
    public void ShowDialogRows(bool isAccepted = false, bool isCompleted = false)
    {
        if (!isSelecting && !isAccepted && !isCompleted)
        {
            for (int i = 0; i < dialogRows.Length; i++ )
            {
                string[] cells = dialogRows[i].Split(',');
                if (cells[0] == "Dialog" && int.Parse(cells[1]) == dialogIndex)
                {
                    UpdateText(cells[2], cells[3]);

                    dialogIndex = int.Parse(cells[4]);
                    break;
                }

                else if (cells[0] == "End"  && int.Parse(cells[1]) == dialogIndex)
                {   
                    
                    if (IsAccepted == true)
                    {
                        taskList.IsAccepted = true;
                    }
                    ClosePanel(UIConst.DialogBox);
                }

                else if (cells[0] == "Selecting" && int.Parse(cells[1]) == dialogIndex)
                {
                    GameManager.Instance.inputActions.Player.Fire.started -= OnClickNext;
                    SetSelectButton(dialogIndex);
                }
            }
        }

        else if (!isSelecting && isAccepted && !isCompleted)
        {
            for (int i = 0; i < dialogRows.Length; i++)
            {
                string[] cells = dialogRows[i].Split(',');
                if (cells[0] == "Accepting")
                {
                    UpdateText(cells[2], cells[3]);

                    dialogIndex = int.Parse(cells[4]);
                    break;
                }
                else if (cells[0] == "End"  && int.Parse(cells[1]) == dialogIndex)
                {   
                    ClosePanel(UIConst.DialogBox);
                }
            }
        }
        else
        {
            for (int i = 0; i < dialogRows.Length; i++)
            {
                string[] cells = dialogRows[i].Split(',');
                if (cells[0] == "Complete")
                {
                    UpdateText(cells[2], cells[3]);

                    dialogIndex = int.Parse(cells[4]);
                    GameManager.Instance.PostTask(taskList.taskID);
                    break;
                }
                else if (cells[0] == "End"  && int.Parse(cells[1]) == dialogIndex)
                {   
                    ClosePanel(UIConst.DialogBox);
                }
            }
        }
    }

    private void SetSelectButton(int id)
    {
        for (int i = id; i < dialogRows.Length; i++ )
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "Selecting")
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

    public void OnClickNext(InputAction.CallbackContext context)
    {
        ShowDialogRows();
    }
}
