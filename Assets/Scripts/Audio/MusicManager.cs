using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    private readonly string[] sceneNames = {"MenuScene", "Suntail Village test", "SampleScene", "KawaiiCity"};
    public AudioClip[] musicClips;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // public void SetMusicGameObject()
    // {
    //     AudioSource audioSource = Instantiate(musicManager, gameObject.transform.position, Quaternion.identity);

    //     audioSource.Play();
    // }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (scene.name == sceneNames[i])
            {
                PlayMusic(musicClips[i]);
                break;
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {   
        AudioSource audioSource = Instantiate(musicManager, gameObject.transform.position, Quaternion.identity);
        if (audioSource.clip != clip)
        {

            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

enum MusicStats
{
    スローライフに憧れて,
    Lazy_Afternoon,
}