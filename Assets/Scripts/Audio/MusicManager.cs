using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    private void Start()
    {
    }

    public void SetMusicGameObject()
    {
        AudioSource audioSource = Instantiate(musicManager, gameObject.transform.position, Quaternion.identity);

        audioSource.Play();
    }
}
