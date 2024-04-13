using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Transform UIStart;
    private Transform UIContinue;
    private Transform UIExit;
    private LoadingScene loadingPanel;
    private void Start() 
    {
        InitUI();    
        InitClick();
        GameManager.Instance.InitBaseInput();
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIStart = transform.Find("Content/Start");
        UIContinue = transform.Find("Content/Continue");
        UIExit = transform.Find("Content/Exit");
    }

    private void InitClick()
    {
        UIStart.GetComponent<Button>().onClick.AddListener(OnClickStart);
        UIContinue.GetComponent<Button>().onClick.AddListener(OnClickContinue);
        UIExit.GetComponent<Button>().onClick.AddListener(OnClickExit);
    }

    private void OnClickStart()
    {
        SceneController.Instance.EnterFirstScene("Scenes/SampleScene");
    }

    private void OnClickContinue()
    {
        print("-------load------");
    }

    private void OnClickExit()
    {
        print("-------Exit------");
    }

}
