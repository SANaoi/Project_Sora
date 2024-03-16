using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarCell : MonoBehaviour
{

    public Transform HealthMask;
    void Awake()
    {
        InitUI();


    }

    void InitUI()
    {   
        HealthMask = transform.Find("barMask");
    }

    public void RefreshUI(int currentHealth, int maxHralth)
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        float sliderPercent = 1 - (float)currentHealth / maxHralth;
        HealthMask.GetComponent<Image>().fillAmount = sliderPercent;
    }
}
