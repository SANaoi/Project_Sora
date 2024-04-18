using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff/BurnBuff", fileName = "BurnBuff")]
public class BurnBuff_SO : Buff_SO
{
    public int tickDamage;

    public override void UpdateBuff(GameObject target)
    {
        base.UpdateBuff(target);

        if (isBuffActive)
        {
            Debug.Log("燃烧");
        }
    }
}
