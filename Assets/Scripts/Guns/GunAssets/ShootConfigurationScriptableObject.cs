using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDX.Collections.Generic;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.15f;
    public float Damage = 5;
    public int Capacity = 30;
    public int bulletTypeID;
    public BuffType GunBuffType;
    public float buildAmount;
}  
