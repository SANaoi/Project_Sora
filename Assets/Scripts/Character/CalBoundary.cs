using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalBoundary : MonoBehaviour
{
    Vector3 Location;
    float height;
    float width;
    float length;
    // Start is called before the first frame update
    void Start()
    {
        Location = transform.localScale;
        height = Location.y;
        width = Location.x;
        length = Location.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnDrawGizmos()
    // {
    //     // 获取对象的Collider组件
    //     Collider collider = GetComponent<Collider>();

    //     if (collider != null)
    //     {
    //         // 获取对象的包围框信息
    //         Bounds bounds = collider.bounds;

    //         // 设置Gizmos的颜色为红色
    //         Gizmos.color = Color.red;

    //         // 绘制包围框
    //         Gizmos.DrawWireCube(bounds.center, bounds.size);

    //         Vector3 center = bounds.center;
    //         Vector3 ext = bounds.extents;

    //         float deltaX = Mathf.Abs(ext.x);
    //         float deltaY = Mathf.Abs(ext.y);
    //         float deltaZ = Mathf.Abs(ext.z);

    //         #region 获取AABB包围盒顶点
    //         Vector3[] points = new Vector3[8];
    //         points[0] = center + new Vector3(-deltaX, deltaY, -deltaZ);        // 上前左（相对于中心点）
    //         points[1] = center + new Vector3(deltaX, deltaY, -deltaZ);         // 上前右
    //         points[2] = center + new Vector3(deltaX, deltaY, deltaZ);          // 上后右
    //         points[3] = center + new Vector3(-deltaX, deltaY, deltaZ);         // 上后左

    //         points[4] = center + new Vector3(-deltaX, -deltaY, -deltaZ);       // 下前左
    //         points[5] = center + new Vector3(deltaX, -deltaY, -deltaZ);        // 下前右
    //         points[6] = center + new Vector3(deltaX, -deltaY, deltaZ);         // 下后右
    //         points[7] = center + new Vector3(-deltaX, -deltaY, deltaZ);        // 下后左
    //         #endregion
    //     }
    // }
    
}
