
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;
using System.Linq;
using Cinemachine;
[RequireComponent(typeof(CharacterStats))]
public class PlayerManager : MonoBehaviour
{
    Rigidbody rig;
    Camera mainCamera;
    public Transform LookPoint;
    bool isWalk;
    bool isArmRifle;
    public bool isJumping;
    [HideInInspector]
    public bool isAiming;
    float backwardSpeed = -1.3f;
    float currentSpeed = 0f;
    float WalkSpeed = 1.5f;
    float RunSpeed = 4f;
    float baseSpeed;

    Vector3 currentTargetRotation;
    Vector3 timeToReachTargetRotation;
    Vector3 dampedTargetRotationCurrentVelocity;
    Vector3 dampedTargetRotationPassedTime;
    Vector2 playerMoveContext;

    [HideInInspector]
    public PlayerPostureState playerPosture;
    [HideInInspector]
    public PlayerMoveState playerMoveState;
    [HideInInspector]
    public PlayerArmState playerArmState = PlayerArmState.Normal;
    public Animator animator;
    private ShootController shootController;

    private Transform[] AllChildrenList;
    private GameObject HandleOnHand;
    private GameObject HandleOnBack;
    private TwoBoneIKConstraint rightHandConstraint;
    private TwoBoneIKConstraint leftHandConstraint;
    private MultiAimConstraint AimingConstraint;
    private MultiAimConstraint AimingIdleConstraint;
    private MultiAimConstraint HeadConstraint;
    public AudioSource shootAudio;
    private CharacterController characterController;
    // 记录上一帧人物速度
    private Vector3 moveDirection = Vector3.zero;
    [HideInInspector]
    public float gravity = -9.8f;
    [HideInInspector]
    public float jumpVelocity = 3f;
    private CinemachineVirtualCamera virtualCamera;
    private Vector3 moveVelocity; // 平滑速度向量

    private GameObject aimTarget;
    private AimingController aimingController;

    // 场景可获取物品功能参数
    private List<GameObject> _gameObjectList;
    public List<GameObject> gameObjectList
    {
        get 
        {   
            return _gameObjectList;
        }
        set
        {
            _gameObjectList = value;
        }
    }
    [HideInInspector]
    public string SelectingID = "";
    private float searchRadius;
    #region 玩家姿态及相关动画参数阈值
    public enum PlayerPostureState
    {
        Stand,
        Crouch,
        midAir
    };
    #endregion

    #region 玩家运动状态
    public enum PlayerMoveState
    {
        backwardSpeed,
        Idle,
        Walk,
        Run
    };
    #endregion

    #region 玩家装备状态
    public enum PlayerArmState
    {
        Normal,
        Rifle,
        Aim
    };
    #endregion

    #region Mono自带类
    void Awake()
    {   
        rig =                   GetComponent<Rigidbody>();
        shootController =       GetComponent<ShootController>();
        characterController =   GetComponent<CharacterController>();

        mainCamera = Camera.main;


        RegisterPlayer();
    }


    void Start()
    {   
        
        InitDicts();
        InitGameObjects();
        InitDelegate();

        UpdatePackageLocalData();
        animator =              GetComponent<Animator>();
        animator.SetFloat("ScaleFactor", 1 / animator.humanScale);

        playerMoveState = PlayerMoveState.Run;
        playerPosture = PlayerPostureState.Stand;
        searchRadius = 2.0f;

        isWalk = false;
        isArmRifle = false;
        isJumping = false;
        isAiming = false;

        // UIManager.Instance.OpenPanel(UIConst.ItemsInfo);
        UIManager.Instance.OpenPanel(UIConst.PlayerMainUI);
        UIManager.Instance.OpenPanel(UIConst.GunInfo);
        InvokeRepeating("SearchForGameObjectWithTag", 0.25f, 0.25f);
        // 确保 gunAudio 组件存在
        if (shootAudio == null)
        {
            shootAudio = gameObject.AddComponent<AudioSource>();
        }
        if (aimingController == null)
        {
            aimingController = FindAnyObjectByType<AimingController>();
        }
        // print(this.name + "  start");
        InitInputSystem();
    }

    private void OnEnable()
    {
        // print(this.name + "  OnEnable  ");
        
    }

    public void RegisterPlayer()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            virtualCamera.Follow = LookPoint;
            virtualCamera.LookAt = LookPoint;
        }
    }

    private void OnDisable()
    {
        // print(this.name + "  OnDisable");
        LogoutInputSystem();
        animator = null;
        gameObjectList.Clear();
    }
    void Update()
    {
        SwitchStates();
        UpdateConstraintWeight();
        Move();
        animator.SetBool("Rifle", isArmRifle);
        animator.SetBool("Aiming", isAiming);
    }
    void LateUpdate()
    {
        UpdateAimingTransform();
    }
    #endregion

    #region Main Methods
    private void InitDicts()
    {
        _gameObjectList = new List<GameObject>();
    }

    private void InitGameObjects()
    {
        AllChildrenList = GetComponentsInChildren<Transform>();
        foreach (Transform ChildObj in AllChildrenList)
        {   
            if (ChildObj.name == "aimTarget")
            {
                aimTarget = ChildObj.GameObject();
            }
            if (ChildObj.name == "Handle OnBack")
            {
                HandleOnBack = ChildObj.GameObject();
            }
            if (ChildObj.name == "Handle OnHand")
            {
                HandleOnHand = ChildObj.GameObject();
                HandleOnHand.SetActive(false);
            }
            if (ChildObj.name == "Two Hand Rig")
            {
                rightHandConstraint = ChildObj.Find("Right Hand Constraint").GetComponent<TwoBoneIKConstraint>();
                leftHandConstraint = ChildObj.Find("Left Hand Constraint").GetComponent<TwoBoneIKConstraint>();
            }
            if (ChildObj.name == "Aiming Chest Rig")
            {
                AimingConstraint = ChildObj.Find("Aiming Constraint").GetComponent<MultiAimConstraint>();
            }
            if (ChildObj.name == "Aiming Body Rig")
            {
                AimingIdleConstraint = ChildObj.Find("Aiming Idle Constraint").GetComponent<MultiAimConstraint>();
            }
            if (ChildObj.name == "Aiming Head Rig")
            {
                HeadConstraint = ChildObj.Find("Head Constraint").GetComponent<MultiAimConstraint>();
            }
        }
    }

    private void InitInputSystem()
    {
        GameManager.Instance.inputActions.Player.Move.performed += GetplayerMoveInput;
        GameManager.Instance.inputActions.Player.Move.canceled += CancelPlayerMoveInput;
        GameManager.Instance.inputActions.Player.Jump.performed += GetJumpInput;
        GameManager.Instance.inputActions.Player.PickUp.canceled += GetPickUpInput;
        GameManager.Instance.inputActions.Player.Aiming.performed += GetAimingInput;
        GameManager.Instance.inputActions.Player.Reload.performed += GetReloadInput;
        GameManager.Instance.inputActions.Player.Rifle.performed += GetArmRifleInput;
        GameManager.Instance.inputActions.Player.Crouch.performed += GetPostureStateInput;
        GameManager.Instance.inputActions.Player.SelectItem.performed += GetSelectItemInput;
        GameManager.Instance.inputActions.Player.WalkToggle.performed += GetWalkToggleInput;

        GameManager.Instance.inputActions.Player.Aiming.performed += aimingController.SwitchCameraParameter;
        GameManager.Instance.inputActions.Player.Fire.started += shootController.OnFireStarte;
        GameManager.Instance.inputActions.Player.Fire.canceled += shootController.OnFireCancel;
    }

    private void LogoutInputSystem()
    {   
        GameManager.Instance.inputActions.Player.Move.performed -= GetplayerMoveInput;
        GameManager.Instance.inputActions.Player.Move.canceled -= CancelPlayerMoveInput;
        GameManager.Instance.inputActions.Player.Jump.performed -= GetJumpInput;
        GameManager.Instance.inputActions.Player.PickUp.canceled -= GetPickUpInput;
        GameManager.Instance.inputActions.Player.Aiming.performed -= GetAimingInput;
        GameManager.Instance.inputActions.Player.Reload.performed -= GetReloadInput;
        GameManager.Instance.inputActions.Player.Rifle.performed -= GetArmRifleInput;
        GameManager.Instance.inputActions.Player.Crouch.performed -= GetPostureStateInput;
        GameManager.Instance.inputActions.Player.SelectItem.performed -= GetSelectItemInput;
        GameManager.Instance.inputActions.Player.WalkToggle.performed -= GetWalkToggleInput;

        GameManager.Instance.inputActions.Player.Aiming.performed -= aimingController.SwitchCameraParameter;
        GameManager.Instance.inputActions.Player.Fire.started -= shootController.OnFireStarte;
        GameManager.Instance.inputActions.Player.Fire.canceled -= shootController.OnFireCancel;
    }

    void InitDelegate()
    {
        EventCenter.Instance.AddEventListener("ActiveInputSystem", InitInputSystem);
        EventCenter.Instance.AddEventListener("LogoutInputSystem", LogoutInputSystem);
    }

    private void UpdateConstraintWeight()
    {
        SetTowHandsWeight();

        SetAimingRiggingConstraintWeight();
        SetAimingIdleConstraintWeight();
        SetHeadConstraintWeight();
    }

    private void SwitchStates()
    {
        SwitchPlayerMoveStates();
        SwitchPlayerArmStates();
        SwitchPlayerPostureStates();
    }


    public void UpdatePackageLocalData()
    {
        // 用于及时更新与界面UI有关的数据
        shootController.TotalAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2);
        UIManager.Instance.OpenPanel("GunInfo").GetComponent<UIGunInfo>().Refresh(shootController.MagazineAmmo,shootController.TotalAmmo);
    }

    public void UpdateAimingTransform()
    {
        aimTarget.transform.position = aimingController.UpdateLookPoint();
    }

    private void HorizontalVelocityCalculate()
    {
        if (playerMoveContext == Vector2.zero)
        {
            ResetVelocity();
            return;
        }
        Vector3 movementDirection = GetMovementInputDirection();

        float targetRotationYAngle = Rotate(movementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        
        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        animator.SetFloat("Speed", movementSpeed, 0.1f, Time.deltaTime);
        if (!isAiming)
        {
            Vector3 tep = targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity;
            Vector3 targetMoveDirection = new Vector3(tep.x, 0f, tep.z);
            moveDirection = Vector3.SmoothDamp(moveDirection, targetMoveDirection, ref moveVelocity, 0.0f);
        }
        else
        {
            Vector3 AimingmovementDirection = GetPalyerMoveVector();
            Vector3 tep = AimingmovementDirection * Mathf.Abs(movementSpeed) - currentPlayerHorizontalVelocity;
            Vector3 targetMoveDirection = new Vector3(tep.x, 0f, tep.z);
            
            moveDirection = Vector3.SmoothDamp(moveDirection, targetMoveDirection, ref moveVelocity, 0.0f);
        }
    }
    private void Move()
    {
        if (isAiming)
        {
            ChangePlayerRotation();
        }
        if (characterController.isGrounded)
        {   
            HorizontalVelocityCalculate();

            moveDirection.y = 0.0f;
            if (isJumping)
            {
                moveDirection.y = jumpVelocity;
                animator.SetFloat("VerticalSpeed", jumpVelocity);
            }
        }
        
        RotateTowardsTargetRotation();
        moveDirection.y += gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        if (!characterController.isGrounded)
        {
            if (isJumping)
            {
                animator.Play("Jump Tree");
                playerPosture = PlayerPostureState.midAir; 
            }
            else if (moveDirection.y <= -1f)
            {
                animator.Play("Jump Tree");
                playerPosture = PlayerPostureState.midAir;
                isJumping = true;
            }
        }

        if (characterController.isGrounded && isJumping)
        {
            isJumping = false;
            animator.Play("Base Tree");
        }
    }


    public void PutGrabRifle(int isOnback)
    {
        if (isOnback == 1)
        {
            HandleOnHand.SetActive(false);
            HandleOnBack.SetActive(true);
        }
        else if (isOnback == 0)
        {
            HandleOnHand.SetActive(true);
            HandleOnBack.SetActive(false);
        }
    }

    void SetTowHandsWeight()
    {
        rightHandConstraint.weight = animator.GetFloat("Right Hand Weight");
        leftHandConstraint.weight = animator.GetFloat("Left Hand Weight");
    }

    void SetAimingRiggingConstraintWeight()
    {
        if (isAiming && playerMoveContext != Vector2.zero)
        {
            AimingConstraint.weight = 1f;
        }
        else
        {
            AimingConstraint.weight = 0f;
        }
    }
    private void SetAimingIdleConstraintWeight()
    {
        if (isAiming && playerMoveContext == Vector2.zero)
        {
            transform.rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
            AimingIdleConstraint.weight = 1f;
        }
        else
        {
            AimingIdleConstraint.weight = 0f;
        }
    }
    private void SetHeadConstraintWeight()
    {
        if (isAiming)
        {
            HeadConstraint.weight = 1f;
        }
        else
        {
            HeadConstraint.weight = 0f;
        }
    }

    private void ShootSound_Ak(int isShoot)
    {
        shootAudio.Play();
    }
    
    
    #endregion

    #region 一般状态下的移动及相机调整
    private Vector3 GetMovementInputDirection()
    {
        // 返回表示移动输入方向的Vector3。
        return new Vector3(playerMoveContext.x, 0f, playerMoveContext.y);
    }

    private float Rotate(Vector3 direction)
    {
        // 通过给定的方向来旋转玩家。更新目标旋转角度，使玩家朝向目标旋转，并返回方向角度。
        float directionAngle = UpdateTargetRotation(direction);

        return directionAngle;
    }


    private float AddCameraRotationToAngle(float angle)
    {
        // 将相机的旋转角度添加到给定的角度上，考虑到相机当前的y轴旋转。
        angle += mainCamera.transform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private float GetDirectionAngle(Vector3 direction)
    {
        // Mathf.Atan2: 算出夹角弧度
        // Mathf.Rad2Deg：弧度转度（范围[-pi，pi])
        // 根据输入方向计算角度（以度为单位）
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        //  使用给定的目标角度更新目标旋转数据。
        currentTargetRotation.y = targetAngle;

        dampedTargetRotationPassedTime.y = 0f;
    }
    private float GetMovementSpeed()
    {
        currentSpeed = Mathf.Lerp(baseSpeed, currentSpeed, 0.8f * Time.deltaTime);
        // 返回移动速度，考虑到基础速度和速度修正。
        return currentSpeed;
    }

    private Vector3 GetPlayerHorizontalVelocity()
    {
        // 返回玩家刚体速度的水平分量
        Vector3 playerHorizontalVelocity = rig.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

    private void RotateTowardsTargetRotation()
    {
        // 使用SmoothDampAngle平滑地将玩家旋转到目标旋转。
        float currentYAngle = rig.rotation.eulerAngles.y;
        if (currentYAngle == currentTargetRotation.y)
        {
            return;
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y,
                                                    timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);
        dampedTargetRotationPassedTime.y += Time.deltaTime;
        smoothedYAngle = Mathf.Lerp(currentYAngle, smoothedYAngle, 30f * Time.deltaTime);
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

        rig.MoveRotation(targetRotation);
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        // 根据输入方向更新目标旋转。如果指定，考虑相机旋转。
        float directionAngle = GetDirectionAngle(direction);

        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        // 四元数和向量相乘可以表示这个向量按照这个四元数进行旋转之后得到的新的向量
        // Vector3.forward 表示世界坐标系中的正前方（0, 0, 1）。
        // 它表示将正前方向量 Vector3.forward 绕 y 轴旋转了 targetAngle 度后的方向。
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void ResetVelocity()
    {
        currentSpeed = Mathf.Lerp(0f, currentSpeed, 0.8f);
        if (currentSpeed <= 0.1f)
        {
            currentSpeed = 0f;
        }
        moveDirection = new Vector3(0f, moveDirection.y, 0f);
        animator.SetFloat("Speed", currentSpeed);
    }

    #endregion

    #region 瞄准时玩家的移动逻辑
    private Vector3 GetPalyerMoveVector()
    {
        Vector3 moveDirection = playerMoveContext.x * transform.right + playerMoveContext.y * transform.forward;
        return moveDirection.normalized;
    }
    void ChangePlayerRotation()
    {
        transform.transform.rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);

    }
    #endregion

    #region 玩家运动状态
    void SwitchPlayerMoveStates()
    {
        if (playerMoveContext == Vector2.zero)
            playerMoveState = PlayerMoveState.Idle;

        else if (playerMoveContext.y < 0 && isAiming)
            playerMoveState = PlayerMoveState.backwardSpeed;
        else if (playerMoveContext.y >= 0 && isAiming)
            playerMoveState = PlayerMoveState.Walk;
        else
            playerMoveState = isWalk ? PlayerMoveState.Walk : PlayerMoveState.Run;

        switch (playerMoveState)
        {
            case PlayerMoveState.backwardSpeed:
                baseSpeed = backwardSpeed;
                break;
            case PlayerMoveState.Idle:
                baseSpeed = 0f;
                break;
            case PlayerMoveState.Walk:
                baseSpeed = WalkSpeed;
                break;
            case PlayerMoveState.Run:
                baseSpeed = RunSpeed;
                break;
        }
    }
    #endregion

    #region 切换人物姿态
    void SwitchPlayerPostureStates()
    {
        switch (playerPosture)
        {
            case PlayerPostureState.Stand:
                animator.SetBool("isCrouch", false);
                break;
            case PlayerPostureState.Crouch:
                animator.SetBool("isCrouch", true);
                break;
            case PlayerPostureState.midAir:
                animator.SetFloat("VerticalSpeed", moveDirection.y, 0.9f, Time.deltaTime);
                break;
        }
    }
    #endregion

    #region 切换玩家装备状态
    void SwitchPlayerArmStates()
    {

        switch (playerArmState)
        {
            case PlayerArmState.Normal:
                animator.SetBool("Rifle", isArmRifle);
                break;
            case PlayerArmState.Rifle:
                animator.SetBool("Rifle", isArmRifle);
                break;
            case PlayerArmState.Aim:
                animator.SetBool("Aiming", isAiming);
                break;
        }

    }
    #endregion

    #region 检索周围可拾取物品
    private void SearchForGameObjectWithTag()
    {   
        var foundObjects = Physics.OverlapSphere(transform.position, searchRadius);
        List<GameObject> foundItems = new();
        List<GameObject> ToBeDeleted = new();
        ItemsInfo itemsInfo = UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>();
        foreach (var foundObject in foundObjects)
        {
            if (foundObject.CompareTag("Item") || foundObject.CompareTag("NPC"))
            {
                foundItems.Add(foundObject.gameObject);
                string NowUid = foundObject.GetComponent<ItemCell>().uid;
                if (!gameObjectList.Any(gameObject => gameObject.GetComponent<ItemCell>().uid == NowUid))
                {
                    gameObjectList.Add(foundObject.gameObject);
                    itemsInfo.GetScrollContent(foundObject.gameObject);
                }
            }
        }
        foreach (GameObject foundObject in gameObjectList)
        {
            string uid = foundObject.GetComponent<ItemCell>().uid;
            if (!foundItems.Any(gameObject => gameObject.GetComponent<ItemCell>().uid == uid))
            {
                ToBeDeleted.Add(foundObject.gameObject);
            }
        }
        foreach (GameObject obj in ToBeDeleted)
        {
            gameObjectList.Remove(obj);
            itemsInfo.DelScrollContent(obj);
        }

        // 刷新选中的物品
        if (gameObjectList.Count == 0)
        {
            SelectingID = "";
            return;
        }
        if (SelectingID != "")
        {
            for (int i = 0; i < itemsInfo.scrollContent.childCount; i++)
            {
                ItemDetail temp = itemsInfo.scrollContent.GetChild(i).GetComponent<ItemDetail>();
                if (temp.UISelecting.gameObject.activeSelf == true)
                {
                    return;
                }
            }
        }
        SelectingID = itemsInfo.scrollContent.GetChild(0).GetComponent<ItemDetail>().uid;
        itemsInfo.scrollContent.GetChild(0).GetComponent<ItemDetail>().UISelecting.gameObject.SetActive(true);

    }
    #endregion
    
    #region InputSystem Methods
    private void GetWalkToggleInput(InputAction.CallbackContext context)
    {
        isWalk = !isWalk;
    }

    public void GetArmRifleInput(InputAction.CallbackContext context)
    {
        isArmRifle = !isArmRifle;
        if (isArmRifle)
        {
            playerArmState = PlayerArmState.Rifle;
        }
        else
        {
            isAiming = false;
            playerArmState = PlayerArmState.Normal;
        }

    }
    public void GetAimingInput(InputAction.CallbackContext context)
    {
        isAiming = !isAiming;
        if (isAiming)
        {
            isArmRifle = true;
            playerArmState = PlayerArmState.Aim;
        }
        else
        {
            playerArmState = PlayerArmState.Rifle;
        }
    }

    private void GetPostureStateInput(InputAction.CallbackContext context)
    {
        if (playerPosture == PlayerPostureState.Stand)
        {
            playerPosture = PlayerPostureState.Crouch;
        }
        else
        {
            playerPosture = PlayerPostureState.Stand;
        }
    }

    private void GetJumpInput(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
    }

    private void GetSelectItemInput(InputAction.CallbackContext context)
    {
        if (SelectingID == "" || gameObjectList.Count == 0)
        {
            return;
        }
        if (context.ReadValueAsButton())
        {
            // 上移
            UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().UpSelectID();
        }
        else
        {
            // 下移
            UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().DownSelectID();
        }
    }

    private void GetplayerMoveInput(InputAction.CallbackContext context)
    {
        playerMoveContext = context.ReadValue<Vector2>();
    }

    private void CancelPlayerMoveInput(InputAction.CallbackContext context)
    {
        playerMoveContext = Vector2.zero;
    }

    private void GetPickUpInput(InputAction.CallbackContext context)
    {
        if (SelectingID != "" && gameObjectList.Count != 0)
        {
            foreach (GameObject Item in gameObjectList)
            {
                ItemCell ItemInfo = Item.GetComponent<ItemCell>();
                if (ItemInfo.uid == SelectingID && ItemInfo.type == 1) // 1为可拾取的场景物体
                {
                    int ItemNum = UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().GetItemNumByUID(SelectingID);
                    if (ItemNum == UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().scrollContent.childCount - 1)
                    {

                        UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().UpSelectID();
                        PackageLocalData.Instance.AddPackageLocalItem(ItemInfo);
                        UpdatePackageLocalData();
                        ItemInfo.Destroy();
                        return;
                    }
                    else
                    {
                        UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().DownSelectID();
                        PackageLocalData.Instance.AddPackageLocalItem(ItemInfo);
                        UpdatePackageLocalData();
                        ItemInfo.Destroy();
                        return;
                    }
                }
                else if (ItemInfo.uid == SelectingID && ItemInfo.type == 101)
                {
                    NPCController nPC = Item.GetComponent<NPCController>();
                    nPC.ShowDialog();
                }

                else if (ItemInfo.uid == SelectingID && ItemInfo.type == 102)
                {
                    TransitionPoint transitionPoint = ItemInfo.transform.gameObject.GetComponent<TransitionPoint>();
                    SceneController.Instance.TransitionToDestination(transitionPoint);
                }
            }
        }
    }

    private void GetReloadInput(InputAction.CallbackContext context)
    {
        if (shootController.MagazineAmmo < shootController.ShootConfig.Capacity && shootController.TotalAmmo != 0)
        {
            RefreshGunInfo();
            animator.SetTrigger("Reload");
            // UpdatePackageLocalData();
        }
    }

    public void RefreshGunInfo()
    {
        // TODO 数据同步到背包数据
        int TotalAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2); // 弹药总数

        if (shootController.MagazineAmmo + TotalAmmo < shootController.ShootConfig.Capacity)
        {
            shootController.MagazineAmmo += TotalAmmo;
            GameManager.Instance.DiscountPackageLocalItemsNumById(2, TotalAmmo);
            shootController.TotalAmmo = 0;
        }
        else
        {
            int ReloadAmmo = shootController.ShootConfig.Capacity - shootController.MagazineAmmo; // 装填弹药量
            GameManager.Instance.DiscountPackageLocalItemsNumById(2, ReloadAmmo);
            shootController.MagazineAmmo = shootController.ShootConfig.Capacity;
            print(3 +" "+ TotalAmmo);
        }
        int leftAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2);
        shootController.TotalAmmo = leftAmmo;
        UIManager.Instance.OpenPanel("GunInfo").GetComponent<UIGunInfo>().Refresh(shootController.MagazineAmmo, shootController.TotalAmmo);
    }
    #endregion

    
    // public Animator animator;
    [Range(0, 1)]
    public float RightWeight;
    [Range(0, 1)]
    public float LeftWeight;
    // public Vector3 LeftPosition;
    // public Vector3 RightPosition;
    private void OnAnimatorIK(int layerIndex)
    {
      
        // animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftWeight);
        // animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftWeight);

        // animator.SetIKPositionWeight(AvatarIKGoal.RightHand, RightWeight);
        // animator.SetIKRotationWeight(AvatarIKGoal.RightHand, RightWeight);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LeftWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, LeftWeight);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, RightWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, RightWeight);

        // animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.transform.position);
        // animator.SetIKPosition(AvatarIKGoal.RightHand,RightHandTarget.transform.position);

        //  animator.SetIKPosition(AvatarIKGoal.LeftFoot,LeftFootPosition);
        //  animator.SetIKPosition(AvatarIKGoal.RightFoot,RightFootPosition);
    }
}
