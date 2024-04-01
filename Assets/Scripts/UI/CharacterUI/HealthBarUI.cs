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
    private EnemyController enemy;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        enemy = GetComponent<EnemyController>();
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
        
        UIbar.GetComponent<HealthBarCell>().RefreshUI(currentHealth,maxHralth);  
        if (currentHealth <= 0)
        {
            enemy.DestroyObject();
            this.enabled = false;
            return;
        }
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
