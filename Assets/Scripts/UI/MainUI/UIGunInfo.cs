using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIGunInfo : BasePanel
{
    public Transform UICapacity;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UICapacity = transform.Find("Capacity");
    }

    public void Refresh(int MagazineAmmo, int TotalAmmo)
    {
        UICapacity.GetComponent<Text>().text = $"{MagazineAmmo} / {TotalAmmo}";
    }
}
