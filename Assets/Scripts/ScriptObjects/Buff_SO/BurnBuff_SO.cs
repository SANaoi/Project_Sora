using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff/BurnBuff", fileName = "BurnBuff")]
public class BurnBuff_SO : Buff_SO
{
    public int tickDamage;
    private CharacterStats characterStats;


    public override void UpdateBuff(GameObject target)
    {
        base.UpdateBuff(target);

        if (isBuffActive)
        {
            characterStats = target.GetComponent<CharacterStats>();
            characterStats.CurrentHealth -= 2;
            characterStats.UpdateHealthBarOnAttack?.Invoke(characterStats.CurrentHealth, characterStats.MaxHealth);
        }
    }
}
