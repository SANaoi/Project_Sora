using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDX.Collections.Generic;

public class BuffController : MonoBehaviour
{
    [SerializeField] private GameObject BuffCellPrefab;
    private Camera mainCamera;
    [SerializeField] private Dictionary<Buff_SO, BuffCell> BuffCellDict;
    [SerializeField] private SerializableDictionary<BuffType, Sprite> BuffIconDict;

    private BuffManager buffManagerRef;

    void Start()
    {
        mainCamera = Camera.main;
        buffManagerRef = GetComponentInParent<BuffManager>();

        BuffCellDict = new Dictionary<Buff_SO, BuffCell>();
        buffManagerRef.ActivateStatus += OnActivateBuff;
        buffManagerRef.UpdateStatusBuff += OnUpdateBuff;
        buffManagerRef.DeactivateStatusBuff += OnDeactivateBuff;
    }

    private void LateUpdate() 
    {
        transform.forward = -mainCamera.transform.forward;
    }

    private void OnActivateBuff(Buff_SO buff_, float buildAmount)
    {
        BuffCell BuffCellCache = CreateBuffCell(buff_); 
        BuffCellDict[buff_] = BuffCellCache;
        
        BuffCellDict[buff_].isMaskUI.gameObject.SetActive(true);

        OnUpdateBuff(buff_, buildAmount, 0);

    }
    private void OnUpdateBuff(Buff_SO buff_, float buildAmount, float duration)
    {
        BuffCellDict[buff_].BuffProgress.fillAmount = buildAmount;
        BuffCellDict[buff_].BuffDuration.fillAmount = duration;
        if (buffManagerRef.enabledBuffs.ContainsKey(buff_.buffType))
        {
            if (buffManagerRef.enabledBuffs[buff_.buffType].isBuffActive)
            {
                BuffCellDict[buff_].isMaskUI.gameObject.SetActive(false);
            }
        }
    }

    private void OnDeactivateBuff(Buff_SO buff_)
    {
        BuffCellDict[buff_].gameObject.SetActive(false);
    }

    private BuffCell CreateBuffCell(Buff_SO buff_)
    {
        if (BuffCellDict.ContainsKey(buff_))
        {
            BuffCellDict[buff_].gameObject.SetActive(true);
            return BuffCellDict[buff_];
        }
        GameObject BuffCellObj = Instantiate(BuffCellPrefab, transform);
        BuffCellObj.GetComponent<BuffCell>().InitParameters(BuffIconDict[buff_.buffType]);
        BuffCellObj.SetActive(true);
        return BuffCellObj.GetComponent<BuffCell>();
        
    }
}
