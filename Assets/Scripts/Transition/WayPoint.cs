using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoint : MonoBehaviour
{
    public GameObject img;
    public Transform target;
    public Vector3 offset;
    void LateUpdate()
    {
        WayPointPosition();
    }
    void WayPointPosition()
    {
        float minX = 0;
        float maxX = Screen.width - minX;

        float minY = 0;
        float maxY = Screen.height - minY;

        Vector2 screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        float r = 500f;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);
        float Angle = Vector3.Angle(target.position - Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Angle > 80 && Angle < 100)
        {
            return;
        } 
        if(Vector3.Dot(target.position - Camera.main.transform.position, Camera.main.transform.forward) < 0)
        {

            pos.x = Mathf.Clamp(maxX - pos.x, screenPosition.x - r, screenPosition.x + r);
            if (Camera.main.transform.forward.y > target.position.y)
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
                if (Camera.main.transform.forward.y > target.position.y)
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
        
        img.transform.position = pos;
    }
}
        // Change the meter text to the distance with the meter unit 'm'
        // meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";

            // Target is behind the camera
            // Clamp the position to the screen edges
            // pos.x = Mathf.Clamp(maxX - pos.x, minX + r, maxX - r);
            // pos.y = Mathf.Clamp(target_y, minY, maxY);
            // pos.y = Mathf.Clamp(target_y, screenPosition.y - r, screenPosition.y + r);