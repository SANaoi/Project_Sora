using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalBoundary : MonoBehaviour
{
    Vector3 Location;
    float height;
    float width;
    float length;
    float outfit;
    public Vector3[] points;
    // Start is called before the first frame update
    void Start()
    {
        Location = transform.localScale;
        height = Location.y;
        width = Location.x;
        length = Location.z;
        outfit = 0.5f;
        points = getPoints();
    }

    private Vector3[] getPoints()
    {
        // 获取对象的Collider组件
        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            // 获取对象的包围框信息
            Bounds bounds = collider.bounds;

            // 设置Gizmos的颜色为红色
            Gizmos.color = Color.red;

            Vector3 center = bounds.center;
            Vector3 ext = bounds.extents;

            float deltaX = Mathf.Abs(ext.x + outfit);
            float deltaY = Mathf.Abs(ext.y + outfit);
            float deltaZ = Mathf.Abs(ext.z + outfit);

            #region 获取AABB包围盒顶点
            Vector3[] points = new Vector3[4];
            // points[0] = center + new Vector3(-deltaX, deltaY, -deltaZ);        // 上前左（相对于中心点）
            // points[1] = center + new Vector3(deltaX, deltaY, -deltaZ);         // 上前右
            // points[2] = center + new Vector3(deltaX, deltaY, deltaZ);          // 上后右
            // points[3] = center + new Vector3(-deltaX, deltaY, deltaZ);         // 上后左

            // points[4] = center + new Vector3(-deltaX, -deltaY, -deltaZ);       // 下前左
            // points[5] = center + new Vector3(deltaX, -deltaY, -deltaZ);        // 下前右
            // points[6] = center + new Vector3(deltaX, -deltaY, deltaZ);         // 下后右
            // points[7] = center + new Vector3(-deltaX, -deltaY, deltaZ);        // 下后左

            // points[8] = center + new Vector3(-deltaX, 0, 0);                   // 前中心
            // points[9] = center + new Vector3(deltaX, 0, 0);                    // 后中心
            // points[10] = center + new Vector3(0, 0, -deltaZ);                  // 左中心
            // points[11] = center + new Vector3(0, 0, deltaZ);                   // 右中心
            points[0] = center + new Vector3(-deltaX, 0, 0);                   // 前中心
            points[1] = center + new Vector3(deltaX, 0, 0);                    // 后中心
            points[2] = center + new Vector3(0, 0, -deltaZ);                  // 左中心
            points[3] = center + new Vector3(0, 0, deltaZ);                   // 右中心
            #endregion
            return points;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
    
}
