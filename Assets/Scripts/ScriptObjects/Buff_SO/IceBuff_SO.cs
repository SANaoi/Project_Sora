using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


[CreateAssetMenu(menuName = "Buff/IceBuff", fileName = "IceBuff")]
public class IceBuff_SO : Buff_SO
{
    private EnemyController enemy;
    public override void ApplyBuff(GameObject target)
    {
        base.ApplyBuff(target);

        enemy = target.GetComponent<EnemyController>();
        enemy.speedOffset =  0.5f;
        
    }

    public override void RemoveBuff(GameObject target)
    {
        base.RemoveBuff(target);

        enemy = target.GetComponent<EnemyController>();
        enemy.speedOffset = 1f;
    }
}
