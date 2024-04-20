using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionControl : MonoBehaviour
{
    public Transform barPoint;
    public GameObject ExpressionPrefab;
    
    float minX;
    float maxX;

    float minY;
    float maxY;

    Vector2 screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    float r;

    Vector2 pos;
    float Angle;
        

    void OnEnable()
    {
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                ExpressionPrefab = UIManager.Instance.OpenGameObject(UIConst.Expression, canvas.transform);
            }
        }
    }

    void Start()
    {
        
        minX = 0;
        maxX = Screen.width - minX;

        minY = 0;
        maxY = Screen.height - minY;

        screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        r = 500f;
    }

    void LateUpdate()
    {
        if (ExpressionPrefab != null)
        {
            // ExpressionPrefab.transform.position = barPoint.position;

            WayPointPosition();
        }
    }

    void WayPointPosition()
    {
        pos = Camera.main.WorldToScreenPoint(barPoint.position);
        Angle = Vector3.Angle(barPoint.position - Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Angle > 80 && Angle < 100)
        {
            return;
        } 
        if(Vector3.Dot(barPoint.position - Camera.main.transform.position, Camera.main.transform.forward) < 0)
        {

            pos.x = Mathf.Clamp(maxX - pos.x, screenPosition.x - r, screenPosition.x + r);
            if (Camera.main.transform.forward.y > barPoint.position.y)
            {
                float y_positive = screenPosition.y + Mathf.Sqrt(r * r - (pos.x - screenPosition.x) * (pos.x - screenPosition.x));
                pos.y = Mathf.Clamp(y_positive, screenPosition.y - r, screenPosition.y + r);
            }
            else
            {
                float y_positive = screenPosition.y - Mathf.Sqrt(r * r - (pos.x - screenPosition.x) * (pos.x - screenPosition.x));
                pos.y = Mathf.Clamp(y_positive, screenPosition.y - r, screenPosition.y + r);
            }

        }
        else
        {
            pos.x = Mathf.Clamp(pos.x, screenPosition.x - r, screenPosition.x + r);
            if (screenPosition.x - r < pos.x && pos.x < screenPosition.x + r)
            {
                pos.y = Mathf.Clamp(pos.y, screenPosition.y - r, screenPosition.y + r);
            }
            else
            {
                if (Camera.main.transform.forward.y > barPoint.position.y)
                {
                    float y_positive = screenPosition.y + Mathf.Sqrt(r * r - (pos.x - screenPosition.x) * (pos.x - screenPosition.x));
                    pos.y = Mathf.Clamp(y_positive, screenPosition.y - r, screenPosition.y + r);
                }
                else
                {
                    float y_positive = screenPosition.y - Mathf.Sqrt(r * r - (pos.x - screenPosition.x) * (pos.x - screenPosition.x));
                    pos.y = Mathf.Clamp(y_positive, screenPosition.y - r, screenPosition.y + r);
                }
            }
        }
        
        ExpressionPrefab.transform.position = pos;
    }
}