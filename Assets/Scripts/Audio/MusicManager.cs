using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    private void Start()
    {
    AudioSource audioSource = Instantiate(musicManager, gameObject.transform.position, Quaternion.identity);

    audioSource.Play();
    }
}
