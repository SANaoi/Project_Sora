//using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class AimingController : MonoBehaviour
{
    Camera mainCamera;
    PlayerManager playerMoveController;

    float default_m_ScreenX = 0.45f;
    public PlayerMoveControls inputActions;
    public GameObject LookPointObject;
    public CinemachineVirtualCamera normalCamera;
    private CinemachineFramingTransposer VisualnormalCamera;

    public Sprite Aim;
    public Sprite Reload;
    private Zoom zoom;

    [Range(0, 180f)]
    public float z;
    private static AimingController _instance;
    public static AimingController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AimingController();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        playerMoveController = PlayerManager.Instance;
        zoom = FindAnyObjectByType<Zoom>();
        inputActions = GameManager.Instance.inputActions;
        inputActions.Player.Aiming.performed += SwitchCameraParameter;
        VisualnormalCamera = normalCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        VisualnormalCamera.m_ScreenX = default_m_ScreenX;
    }

    void Update()
    {
        // Shoot();
        UpdateLookPoint();
    }

    void LateUpdate()
    {
    }
    void FixedUpdate()
    {
    }
    void UpdateLookPoint()
    {   
        // 计算移动和旋转的目标位置和朝向
        Vector3 targetPosition = mainCamera.transform.rotation * new Vector3(0f , 0f, z) + mainCamera.transform.position;
        
        Quaternion targetRotation = Quaternion.LookRotation(mainCamera.transform.position - LookPointObject.transform.position);

        // 使用插值平滑过渡位置和旋转
        LookPointObject.transform.position = Vector3.Lerp(LookPointObject.transform.position, targetPosition,  1000f * Time.deltaTime);
        LookPointObject.transform.rotation = Quaternion.Slerp(LookPointObject.transform.rotation, targetRotation, 1000f *Time.deltaTime );
    }

    public void SwitchCameraParameter(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SmoothFOVSwitch());
    }

    IEnumerator SmoothFOVSwitch()
    {
        float targetFOV;
        float target_m_ScreenX;
        float targetZoomDistance;

        bool isAiming = playerMoveController.isAiming;

        // 定义平滑过渡的时间
        float duration = 0.2f;
        float elapsedTime = 0f;

        float initialCameraFOV = normalCamera.m_Lens.FieldOfView;
        float initialCamera_m_ScreenX = VisualnormalCamera.m_ScreenX;
        if (isAiming)
        {
            targetFOV = 40f;
            target_m_ScreenX = 0.2f;
            targetZoomDistance = 1.2f;
        }
        else
        {
            targetFOV = 60f;
            target_m_ScreenX = 0.4f;
            targetZoomDistance = 2f;
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
