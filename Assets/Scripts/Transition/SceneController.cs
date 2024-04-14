using System.Collections;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private LoadingScene loadingPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        loadingPanel = UIManager.Instance.OpenPanel(UIConst.LoadingScene) as LoadingScene;
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        
        }   
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {   
        yield return null;
        if (SceneManager.GetActiveScene().name != sceneName)
        {   
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                float progress = operation.progress;
                loadingPanel.Refresh(progress);
                if (operation.progress >= 0.9f)
                {
                    loadingPanel.Refresh(1f);
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
            
            UIManager.Instance.RefreshManager();
            GameManager.Instance.InitBaseGameObject();
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }

    public void EnterFirstScene(string sceneName)
    {
        GameManager.Instance.inputActions.Enable();
        StartCoroutine(Transition(sceneName));
    }

    public void EnterMenuScene()
    { 
        StartCoroutine(Transition("Scenes/MenuScene", true));
    }

    IEnumerator Transition(string sceneName, bool IsMenuScene = false)
    {
        loadingPanel = UIManager.Instance.OpenPanel(UIConst.LoadingScene) as LoadingScene;
        yield return null;
        if (SceneManager.GetActiveScene().name != sceneName)
        {   
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                float progress = operation.progress;
                loadingPanel.Refresh(progress);
                if (operation.progress >= 0.9f)
                {
                    loadingPanel.Refresh(1f);
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
            
            UIManager.Instance.RefreshManager();
            GameManager.Instance.InitBaseGameObject();
            if (IsMenuScene) GameManager.Instance.InitBaseInput();   

        }

    }
}
