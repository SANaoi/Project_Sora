using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class DialogUI : BasePanel
{
    Transform SpeakerName;
    Transform DialogueContent;
    public TextAsset dialogDataFile;

    bool isFinished;
    int dialogIndex;
    string[] dialogRows;

    
    void Awake()
    {
        InitUI();
    }

    void Start()
    {
        reloadData(dialogDataFile);
        ShowDialogRows();

        GameManager.Instance.inputActions.Player.Fire.started += OnClickNext;
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
        foreach (var row in dialogRows)
        {
            string[] cells = row.Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                UpdateText(cells[2], cells[3]);

                dialogIndex = int.Parse(cells[4]);

                if (cells[0] == "End")
                {
                    isFinished = true;
                }
                break;
            }

            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                
            }
        }
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
