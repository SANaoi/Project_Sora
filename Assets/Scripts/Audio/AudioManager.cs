using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public SoundFXManager soundFXManager;
    public SoundMixerManager soundMixerManager;
    public MusicManager musicManager;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        soundFXManager = GetComponent<SoundFXManager>();
        soundMixerManager = GetComponent<SoundMixerManager>();
        musicManager = GetComponent<MusicManager>();
    }
}
