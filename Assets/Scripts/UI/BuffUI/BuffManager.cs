using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<BuffType, Buff_SO> BuffToApplyDict = new();

    public SerializableDictionary<BuffType, Buff_SO> enabledBuffs = new();
    private Dictionary<BuffType, Buff_SO> BuffCacheDict = new Dictionary<BuffType, Buff_SO>();
    
    [SerializeField, Tooltip("Run the updateCall in Buff_SO every what interval")] private float interval = 0.1f;
    private float currentInterval = 0f;
    private float lastInterval = 0f;

    public UnityAction<Buff_SO, float> ActivateStatus;
    public UnityAction<Buff_SO> DeactivateStatusBuff;
    public UnityAction<Buff_SO, float, float> UpdateStatusBuff;
    void Start()
    {
        
    }


    public void OnBuffTriggerBuildup(BuffType buffType, float buildAmount)
    {
        if (!enabledBuffs.ContainsKey(buffType))
        {   
            var buffToAdd = CreateBuffCellObj(buffType, BuffToApplyDict[buffType]);

            enabledBuffs[buffType] = buffToAdd;

            ActivateStatus?.Invoke(buffToAdd, buffToAdd.GetCurrentDurationNormalized());
        }

        if (!enabledBuffs[buffType].isBuffActive)
        {
            enabledBuffs[buffType].AddBuildup(buildAmount, gameObject);

            UpdateStatusBuff?.Invoke(enabledBuffs[buffType], enabledBuffs[buffType].GetCurrentThresholdNormalized(),
                enabledBuffs[buffType].GetCurrentDurationNormalized());
        }
        else
        {
            int tickDamageAmount = (int)Mathf.Ceil(buildAmount / 4);
        }
    }

    private Buff_SO CreateBuffCellObj(BuffType buffType, Buff_SO buff_)
    {
        if (!BuffCacheDict.ContainsKey(buffType))
        {
            BuffCacheDict[buffType] = Instantiate(buff_);
        }

        return BuffCacheDict[buffType];
    }

    public void UpdateBuffs(GameObject target)
    {
        foreach (var effect in enabledBuffs.ToList())
        {
            effect.Value.UpdateCall(target, interval);

            UpdateStatusBuff?.Invoke(effect.Value, effect.Value.GetCurrentThresholdNormalized(),
                effect.Value.GetCurrentDurationNormalized());

            if (effect.Value.CanStatusVisualBeRemove())
            {
                RemoveBuff(effect.Key);
            }
        }
    }

    public void RemoveBuff(BuffType buffType)
    {
        if (enabledBuffs.ContainsKey(buffType))
        {
            enabledBuffs[buffType].RemoveBuff(gameObject);
            
            DeactivateStatusBuff?.Invoke(enabledBuffs[buffType]);

            enabledBuffs.Remove(buffType);

        }
    }
    private void Update()
    {
        currentInterval += Time.deltaTime;
        if (currentInterval > lastInterval + interval)
        {
            UpdateBuffs(gameObject);
            lastInterval = currentInterval;
        }
    }
}
