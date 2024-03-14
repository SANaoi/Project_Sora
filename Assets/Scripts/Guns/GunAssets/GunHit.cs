using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletStates { HitPlayer, HitEnemy, HitNothing}

public class GunHit : MonoBehaviour
{
    public int damage = 6;
    public BulletStates bulletStates;

    void Start()
    {
        bulletStates = BulletStates.HitNothing;
    }

}
