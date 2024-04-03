using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }

    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;


}
