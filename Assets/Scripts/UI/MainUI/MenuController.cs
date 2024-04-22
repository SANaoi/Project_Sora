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
    private TeamController team;
    private void Start() 
    {
        InitUI();    
        InitClick();
        GameManager.Instance.InitBaseInput();
        team = FindAnyObjectByType<TeamController>();
        print(this.name + "Start--------------------");
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
        
        GameManager.Instance.CreateNewLocalConfig();
        SceneController.Instance.EnterFirstScene("Scenes/SampleScene");
    }

    private void OnClickContinue()
    {
        print("-------load------");
        string sceneName = GameManager.Instance.GetUserData().currentScene;
        if (sceneName != null)
            SceneController.Instance.EnterFirstScene(sceneName);
    }

    private void OnClickExit()
    {
        print("-------Exit------");
    }

}
