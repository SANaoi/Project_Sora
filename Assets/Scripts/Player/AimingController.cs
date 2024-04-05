//using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class AimingController : MonoBehaviour
{
    PlayerManager playerMoveController;
    float default_m_ScreenX = 0.45f;
    public PlayerMoveControls inputActions;
    public CinemachineVirtualCamera normalCamera;
    private CinemachineFramingTransposer VisualnormalCamera;
    public Vector3 LookPointPosition;

    // public Sprite Aim;
    // public Sprite Reload;
    private Zoom zoom;
    private bool isStartSetCamera;

    [Range(0, 180f)]
    public float z;

    private void Awake()
    {
        // print(this.name + " Awake--------------");
    }

    void OnEnable()
    {
        // print(this.name + " OnEnable--------------");
    }
    void OnDisable()
    {
        // inputActions.Player.Aiming.performed -= SwitchCameraParameter;
    }

    void Start()
    {
        // print(this.name + " start--------------");
        if (normalCamera == null)
        {
            normalCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        }
        isStartSetCamera = false;
        VisualnormalCamera = normalCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        VisualnormalCamera.m_ScreenX = default_m_ScreenX;
        inputActions = GameManager.Instance.inputActions;
        zoom = FindAnyObjectByType<Zoom>();
        playerMoveController = FindAnyObjectByType<PlayerManager>();
        StartCoroutine(SmoothFOVSwitch());
    }

    void Update()
    {
        
    }
    void LateUpdate()
    {
        StartSetCamera();
    }
    void FixedUpdate()
    {
    }
    public Vector3 UpdateLookPoint()
    {   
        // 计算移动和旋转的目标位置和朝向
        Vector3 targetPosition = Camera.main.transform.rotation * new Vector3(0f , 0f, z) + Camera.main.transform.position;
        // 使用插值平滑过渡位置和旋转
        LookPointPosition = Vector3.Lerp(LookPointPosition, targetPosition,  1000f * Time.deltaTime);
        return LookPointPosition;
    }
    void StartSetCamera()
    {
        if(isStartSetCamera)
            StartCoroutine(SmoothFOVSwitch());
    }
    public void SwitchCameraParameter(InputAction.CallbackContext ctx)
    {
        isStartSetCamera = true;
    }

    IEnumerator SmoothFOVSwitch()
    {
        isStartSetCamera = false;
        float targetFOV;
        float target_m_ScreenX;
        float targetZoomDistance;

        // 定义平滑过渡的时间
        float duration = 0.2f;
        float elapsedTime = 0f;

        float initialCameraFOV = normalCamera.m_Lens.FieldOfView;
        float initialCamera_m_ScreenX = VisualnormalCamera.m_ScreenX;
        if (playerMoveController.isAiming)
        {
            targetFOV = 40f;
            target_m_ScreenX = 0.2f;
            targetZoomDistance = 1.2f;
        }
        else if (!playerMoveController.isAiming)
        {
            targetFOV = 60f;
            target_m_ScreenX = 0.4f;
            targetZoomDistance = 2f;
        }
        else 
        {
            yield break;
        }

        while (elapsedTime < duration)
        {
            normalCamera.m_Lens.FieldOfView = Mathf.Lerp(initialCameraFOV, targetFOV, elapsedTime / duration);
            VisualnormalCamera.m_ScreenX = Mathf.Lerp(initialCamera_m_ScreenX, target_m_ScreenX, elapsedTime / duration);
            zoom.CameraZoom(targetZoomDistance);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        normalCamera.m_Lens.FieldOfView = targetFOV;
        VisualnormalCamera.m_ScreenX = target_m_ScreenX;
    }
    private void OnGUI()
    {
        playerMoveController = FindAnyObjectByType<PlayerManager>();
        if (playerMoveController.isAiming)
        {
            // 设置十字的颜色
            GUI.color = Color.white;
            //Vector3 screenPosition = mainCamera.WorldToScreenPoint(LookPointObject.transform.position);
            // 获取屏幕中心的坐标
            Vector2 screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            // 设置十字的大小
            float crosshairSize = 20f;
            // 绘制垂直线
            GUI.DrawTexture(new Rect(screenPosition.x - 1, screenPosition.y - crosshairSize / 2, 2, crosshairSize), Texture2D.whiteTexture);
            // 绘制水平线
            GUI.DrawTexture(new Rect(screenPosition.x - crosshairSize / 2, screenPosition.y - 1, crosshairSize, 2), Texture2D.whiteTexture);
        }
    }
}
