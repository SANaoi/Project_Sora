using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionControl : MonoBehaviour
{
    private CharacterStats characterStats;
    public Transform barPoint;
    public GameObject ExpressionPrefab;
    Transform cam;

    void OnEnable()
    {
        characterStats = GetComponent<CharacterStats>();
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace && canvas.transform.name == "World UI")
            {
                ExpressionPrefab = UIManager.Instance.OpenGameObject(UIConst.Expression, canvas.transform);
            }
        }
    }

    void LateUpdate()
    {
        if (ExpressionPrefab != null)
        {
            ExpressionPrefab.transform.position = barPoint.position;
            ExpressionPrefab.transform.forward = -cam.forward;
        }
    }
}
