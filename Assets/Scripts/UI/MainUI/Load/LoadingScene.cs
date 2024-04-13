using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingScene : BasePanel
{
    private Transform UIPercentage;
    private Transform UILoadingSlider;

    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIPercentage = transform.Find("Percentage");
        UILoadingSlider = transform.Find("LoadingSlider");
    }

    public void Refresh(float progress)
    {
        UILoadingSlider.GetComponent<Slider>().value = progress;
        UIPercentage.GetComponent<Text>().text = $"{UILoadingSlider.GetComponent<Slider>().value * 100} %";
    }
}
