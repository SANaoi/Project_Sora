using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioUIManager : BasePanel
{
    private Transform UIBanck;
    private Transform UIMenu;
    public AudioManager audioManager;
    private SoundMixerManager soundMixerManager;
    private Transform masterSlider;
    private Transform soundSlider;
    private Transform musicSlider;
    private LoadingScene loadingPanel;
    private void Awake()
    {   
        InitUI();
    }
    private void Start()
    {
        Refresh();
    }
    private void InitUI()
    {
        InitUIName();
        InitClick();
        GetOherComponent();
    }

    private void GetOherComponent()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("Cont Get audioManager");
        }
        soundMixerManager = audioManager.soundMixerManager;
    }

    private void InitUIName()
    {
        UIBanck = transform.Find("Center/Back");
        UIMenu = transform.Find("Center/Menu");
        masterSlider = transform.Find("Center/music/master/masterSlider");
        soundSlider = transform.Find("Center/music/sound/soundSlider");
        musicSlider = transform.Find("Center/music/music/musicSlider");
    }

    private void InitClick()
    {
        UIBanck.GetComponent<Button>().onClick.AddListener(OnClickBack);
        UIMenu.GetComponent<Button>().onClick.AddListener(OnClickMenu);
    }

    private void OnClickBack()
    {
        UIManager.Instance.ClosePanel(UIConst.AudioUIManager, true);
    }

    private void OnClickMenu()
    {
        GameManager.Instance.SaveCurrentLocalConfig();
        
        SceneController.Instance.EnterMenuScene();
    }

    private void Refresh()
    {
        float masterVolume;
        float soundFXVolume;
        float musicVolume;
        soundMixerManager.audioMixer.GetFloat("masterVolume", out masterVolume);
        soundMixerManager.audioMixer.GetFloat("soundFXVolume", out soundFXVolume);
        soundMixerManager.audioMixer.GetFloat("musicVolume", out musicVolume);

        masterSlider.GetComponent<Slider>().value = Mathf.Pow(10, masterVolume / 20f);
        soundSlider.GetComponent<Slider>().value = Mathf.Pow(10, soundFXVolume / 20f);
        musicSlider.GetComponent<Slider>().value = Mathf.Pow(10, musicVolume / 20f);
    }
}
