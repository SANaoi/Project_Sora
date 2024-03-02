using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundController : MonoBehaviour
{
    AudioSource source;
    public AudioClip[] footSteps;
    public AudioClip[] jumpEfforts;
    public AudioClip[] landing;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        int i = Random.Range(0, footSteps.Length);
        source.PlayOneShot(footSteps[i]);
    }

    public void PlayJumpEffort()
    {
        int i = Random.Range(0, jumpEfforts.Length);
        source.PlayOneShot(jumpEfforts[i]);
    }

    public void PlayLanding()
    {
        int i = Random.Range(0, landing.Length);
        source.PlayOneShot(landing[i]);
    }

}

public class ThirdPersonShooterController : MonoBehaviour
{
    Transform playerTransform;
    Animator animator;
    Transform cameraTransform;
    CharacterController characterController;
    PlayerSoundController playerSoundController;  //角色音效播放脚本

    #region 玩家姿态及相关动画参数阈值
    public enum PlayerPosture
    {
        Crouch,
        Stand,
        Falling,
        Jumping,
        Landing
    };
    [HideInInspector]
    public PlayerPosture playerPosture = PlayerPosture.Stand;

    float crouchThreshold = 0f;
    float standThreshold = 1f;
    float midairThreshold = 2.1f;
    float landingThreshold = 1f;
    #endregion

    //玩家运动状态
    public enum LocomotionState
    {
        Idle,
        Walk,
        Run
    };
    [HideInInspector]
    public LocomotionState locomotionState = LocomotionState.Idle;


    //玩家装备状态
    public enum ArmState
    {
        Normal,
        Aim
    };
    [HideInInspector]
    public ArmState armState = ArmState.Normal;

    //玩家不同状态的运动速度
    float crouchSpeed = 1.5f;
    float walkSpeed = 2.5f;
    float runSpeed = 5.5f;

    #region 输入值
    Vector2 moveInput;
    bool isRunning;
    bool isCrouch;
    bool isAiming;
    bool isJumping;
    #endregion

    #region 状态机参数的哈希值
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int verticalVelHash;
    int feetTweenHash;
    #endregion


    Vector3 playerMovement = Vector3.zero;

    //重力
    public float gravity = -9.8f;

    //垂直方向速度
    float VerticalVelocity;

    //最大跳起高度
    public float maxHeight = 1.5f;

    //滞空左右脚状态
    float feetTween;

    #region 速度缓存池定义
    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentChacheIndex = 0;
    Vector3 averageVel = Vector3.zero;
    #endregion

    //下落时加速度的倍数
    float fallMultiplier = 1.5f;

    //玩家是否着地
    bool isGrounded;

    //玩家是否可以跌落
    bool couldFall;

    //跌落的最小高度，小于此高度不会切换到跌落姿态
    float fallHeight = 0.5f;

    //是否处于跳跃CD状态
    bool isLanding;

    //地标检测射线的偏移量
    float groundCheckOffset = 0.5f;

    //跳跃的CD设置
    float jumpCD = 0.15f;

    //上一帧的动画nornalized时间
    float lastFootCycle = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        playerSoundController = GetComponent<PlayerSoundController>();

        postureHash = Animator.StringToHash("玩家姿态");
        moveSpeedHash = Animator.StringToHash("移动速度");
        turnSpeedHash = Animator.StringToHash("转弯速度");
        verticalVelHash = Animator.StringToHash("垂直速度");
        feetTweenHash = Animator.StringToHash("左右脚");

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        SwitchPlayerStates();
        CaculateGravity();
        Jump();
        CaculateInputDirection();
        SetupAnimator();
        PlayFootStep();
    }

    #region 输入相关
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void GetRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValueAsButton();
    }

    public void GetCrouchInput(InputAction.CallbackContext ctx)
    {
        isCrouch = ctx.ReadValueAsButton();
    }

    public void GetAimInput(InputAction.CallbackContext ctx)
    {
        isAiming = ctx.ReadValueAsButton();
    }

    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        isJumping = ctx.ReadValueAsButton();
    }

    #endregion


    /// <summary>
    /// 用于切换玩家的各种状态
    /// </summary>
    void SwitchPlayerStates()
    {
        if (!isGrounded)
        {
            if(VerticalVelocity > 0)
            {
                playerPosture = PlayerPosture.Jumping;
            }
            else if(playerPosture != PlayerPosture.Jumping)
            {
                if(couldFall)
                {
                    playerPosture = PlayerPosture.Falling;
                }
            }

        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            StartCoroutine(CoolDownJump());
        }
        else if (isLanding)
        {
            playerPosture = PlayerPosture.Landing;
        }
        else if (isCrouch)
        {
            playerPosture = PlayerPosture.Crouch;
        }
        else
        {
            playerPosture = PlayerPosture.Stand;
        }

        if (moveInput.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else if (!isRunning)
        {
            locomotionState = LocomotionState.Walk;
        }
        else
        {
            locomotionState = LocomotionState.Run;
        }

        if (isAiming)
        {
            armState = ArmState.Aim;
        }
        else
        {
            armState = ArmState.Normal;
        }
    }


    /// <summary>
    /// 落地检测
    /// </summary>
    void CheckGround()
    {
        if (Physics.SphereCast(playerTransform.position + (Vector3.up * groundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - characterController.radius + 2 * characterController.skinWidth))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            couldFall = !Physics.Raycast(playerTransform.position, Vector3.down, fallHeight);
        }
    }

    /// <summary>
    /// 计算跳跃CD
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolDownJump()
    {
        landingThreshold = Mathf.Clamp(VerticalVelocity, -10, 0);
        landingThreshold /= 20f;
        landingThreshold += 1f;
        isLanding = true;
        playerPosture = PlayerPosture.Landing;
        playerSoundController.PlayLanding();
        yield return new WaitForSeconds(jumpCD);
        isLanding = false;
    }

    /// <summary>
    /// 计算下落速度
    /// </summary>
    void CaculateGravity()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if(!isGrounded)
            {
                VerticalVelocity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                VerticalVelocity = gravity * Time.deltaTime;
            }
        }
        else
        {
            if (VerticalVelocity <= 0 || !isJumping)
            {
                VerticalVelocity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                VerticalVelocity += gravity * Time.deltaTime;
            }
        }

    }

    /// <summary>
    /// 跳跃方法
    /// </summary>
    void Jump()
    {
        if (playerPosture == PlayerPosture.Stand && isJumping)
        {
            playerSoundController.PlayJumpEffort();
            VerticalVelocity = Mathf.Sqrt(-2 * gravity * maxHeight);
            feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
            feetTween = feetTween < 0.5f ? 1 : -1;
            if (locomotionState == LocomotionState.Run)
            {
                feetTween *= 3;
            }
            else if (locomotionState == LocomotionState.Walk)
            {
                feetTween *= 2;
            }
            else
            {
                feetTween = Random.Range(0.5f, 1f) * feetTween;
            }
        }
    }


    /// <summary>
    /// 计算玩家输入相对于相机的方向
    /// </summary>
    void CaculateInputDirection()
    {
        Vector3 camForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        playerMovement = camForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        playerMovement = playerTransform.InverseTransformVector(playerMovement);
    }


    /// <summary>
    /// 设置动画状态机的参数
    /// </summary>
    void SetupAnimator()
    {
        if (playerPosture == PlayerPosture.Stand)
        {
            animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Crouch)
        {
            animator.SetFloat(postureHash, crouchThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                default:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * crouchSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelHash, VerticalVelocity);
            animator.SetFloat(feetTweenHash, feetTween);
        }
        else if (playerPosture == PlayerPosture.Landing)
        {
            animator.SetFloat(postureHash, landingThreshold, 0.03f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if(playerPosture == PlayerPosture.Falling)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelHash, VerticalVelocity);
        }

        if (armState == ArmState.Normal && playerPosture != PlayerPosture.Jumping)
        {
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            animator.SetFloat(turnSpeedHash, rad, 0.1f, Time.deltaTime);
            playerTransform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
    }


    /// <summary>
    /// 计算前三帧的速度平均值
    /// </summary>
    /// <param name="newVel">当前帧的速度平均值</param>
    /// <returns>平均速度</returns>
    Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }

    /// <summary>
    /// 播放脚步声
    /// </summary>
    void PlayFootStep()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if (locomotionState == LocomotionState.Walk || locomotionState == LocomotionState.Run)
            {
                float currentFootCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f); ;
                if ((lastFootCycle < 0.1 && currentFootCycle >= 0.1) || (currentFootCycle >= 0.6 && lastFootCycle < 0.6))
                {
                    playerSoundController.PlayFootStep();
                }
                lastFootCycle = currentFootCycle;
            }
        }
    }

    /// <summary>
    /// 动画系统的回调方法
    /// </summary>
    private void OnAnimatorMove()
    {

        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            Vector3 playerDeltaMovement = animator.deltaPosition;
            playerDeltaMovement.y = VerticalVelocity * Time.deltaTime;
            characterController.Move(playerDeltaMovement);
            averageVel = AverageVel(animator.velocity);
        }
        else
        {
            averageVel.y = VerticalVelocity;
            Vector3 playerDeltaMovement = averageVel * Time.deltaTime;
            characterController.Move(playerDeltaMovement);
        }
    }
}