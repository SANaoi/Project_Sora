using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
public class SelectCellUI : MonoBehaviour
{
    string m_content;
    int m_jumpTo;
    string m_effect;
    string m_target;

    Transform DialogBox;
    void Start()
    {
        InitClik();
    }

    public void InitParameter(string content, int jumpTo, string effect, string target)
    {   
        m_content = content;
        m_jumpTo = jumpTo;
        m_effect = effect;
        m_target = target;
        DialogBox = UIManager.Instance.OpenPanel(UIConst.DialogBox).transform;
    }

    private void InitClik()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnSelectButton);
    }

    private void OnSelectButton()
    {
        // TODO 与其他模块联动,例如：任务系统
        DialogBox.GetComponent<DialogUI>().dialogIndex = m_jumpTo;
        DialogBox.GetComponent<DialogUI>().isSelecting = false;
        DialogBox.GetComponent<DialogUI>().ShowDialogRows();
        UIManager.Instance.ClosePanel(UIConst.SelectBox);
    }
}
