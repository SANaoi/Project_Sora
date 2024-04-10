using System.Collections;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public GameObject playerPrefab;
    public Animator animator;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
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

        if (SceneManager.GetActiveScene().name != sceneName)
        {   
            yield return SceneManager.LoadSceneAsync(sceneName);
            UIManager.Instance.RefreshManager();
            GameManager.Instance.InitBaseGameObject();
            yield break;
        }
        yield return null;
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
}
