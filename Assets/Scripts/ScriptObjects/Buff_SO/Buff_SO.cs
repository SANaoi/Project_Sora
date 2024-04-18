using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BuffType { Fire, Ice}

public abstract class Buff_SO : ScriptableObject
{
    public BuffType buffType;
    
    public float activateThreshold;
    public float thresholdReductionMultiplier = 1f;
    public float thresholdRedutionEverySecond = 1f;

    public float activateDuration;

    public GameObject visualEffectPrefab;

    private float currentThreshold;
    private float remainingDuration;
    private GameObject vfxPlaying;

    [HideInInspector]public bool isBulidUpOnlyShow;
    [HideInInspector]public bool isBuffActive;

    public float tickInterval = 0.5f;
    private float tickIntervalCD;

    public virtual void AddBuildup(float buildAmount, GameObject target)
    {
        isBulidUpOnlyShow = true;
        currentThreshold += buildAmount;
        if (currentThreshold >= activateThreshold)
        {
            ApplyBuff(target);
        }
    }

    public virtual void ApplyBuff(GameObject target)
    {
        isBuffActive = true;
        remainingDuration = activateDuration;

        if (visualEffectPrefab != null)
        {
            vfxPlaying = Instantiate(visualEffectPrefab, target.transform.position, Quaternion.identity, target.transform);
        }

    }

    public void UpdateCall(GameObject target, float tickAmount)
    {
        if (isBuffActive)
        {
            isBulidUpOnlyShow = false;

            remainingDuration -= tickAmount;

            if (remainingDuration <= 0)
            {
                isBuffActive = false;
            }
        }
        else
        {
            currentThreshold -= tickAmount * thresholdRedutionEverySecond * thresholdReductionMultiplier;
            if (currentThreshold <= 0)
            {
                isBulidUpOnlyShow = false;
            }
        }

        tickIntervalCD += tickAmount;
        if (tickIntervalCD >= tickInterval)
        {
            UpdateBuff(target);
            tickIntervalCD = 0;
        }
    }

    public virtual void UpdateBuff(GameObject target)
    {

    }

    public virtual void RemoveBuff(GameObject target)
    {
        isBuffActive = false;
        currentThreshold = 0;
        remainingDuration = 0;

        if (vfxPlaying != null)
        {
            Destroy(vfxPlaying);
        }
        // other actions to perform when SE is deactivated
    }

    public virtual bool CanStatusVisualBeRemove()
    {
        return !(isBuffActive || isBulidUpOnlyShow);
    }

    public float GetCurrentThresholdNormalized()
    {
        return currentThreshold / activateThreshold;
    }

    public float GetCurrentDurationNormalized()
    {
        return remainingDuration / activateDuration;
    }
}
