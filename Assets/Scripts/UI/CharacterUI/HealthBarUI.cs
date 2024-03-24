using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform barPoint;
    Transform cam;
    GameObject UIbar;
    CharacterStats currentStats;
    private float timeLeft;
    public bool alwaysVisable;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        
    }

    void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = UIManager.Instance.OpenGameObject("HealthBarUI", canvas.transform);
            }
        }
    }
    void Start()
    {
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    void UpdateHealthBar(int currentHealth, int maxHralth)
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
        UIbar.GetComponent<HealthBarCell>().RefreshUI(currentHealth,maxHralth);  
    }
    void LateUpdate()
    {   
        if (UIbar != null)
        {
        UIbar.transform.position = barPoint.position;
        UIbar.transform.forward = -cam.forward;        
        }
        // if (timeLeft <= 0f && !alwaysVisable)
        //     UIbar.gameObject.SetActive(false);
        // else
        //     timeLeft -= Time.deltaTime;
    }
}
