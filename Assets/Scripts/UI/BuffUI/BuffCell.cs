using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCell : MonoBehaviour
{
    public GameObject isMaskUI;
    public Image BuffIcon;
    public Image BuffProgress;
    public Image BuffDuration;

    private void Awake()
    {
        InitUI();
    }

    private void OnEnable()
    {
        isMaskUI.gameObject.SetActive(false);
        BuffProgress.fillAmount = 0;
        BuffDuration.fillAmount = 0;
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        isMaskUI = transform.Find("Mask").gameObject;
        BuffIcon = transform.Find("BuffImage").GetComponent<Image>();
        BuffProgress = transform.Find("Progress").GetComponent<Image>();
        BuffDuration = transform.Find("Bg").GetComponent<Image>();
    }

    public void InitParameters(Sprite BuffSpriteIcon)
    {
        print(this.name + "------------");
        isMaskUI.gameObject.SetActive(true);
        BuffIcon.sprite = BuffSpriteIcon;
        BuffProgress.fillAmount = 0;
        BuffDuration.fillAmount = 0;
    }

}
