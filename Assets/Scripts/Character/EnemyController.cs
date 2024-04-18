using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public enum EnemyStates { GUARD, PATROL, CHASE, BATTLE, DEAD }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(ExpressionControl))]
public class EnemyController : MonoBehaviour
{

    public NavMeshAgent agent;
    public EnemyStates enemyStates;
    public Animator animator;
    private Vector3 guardPos;
    private Quaternion currentRotation;

    private Quaternion guardRotation;
    private Quaternion rotationToGuardPosition;
    protected CharacterStats characterStats;

    private bool isGetGuardPosition = false;
    public GameObject attackTarget = null;
    private GameObject bunker;

    [Header("Basic Settings")]
    public float signtRadius;
    public float battleRadius;
    public bool isGuard;
    public bool isGunToting;
    private float lastAttackTime;
    [Header("Patrol State")]
    public float PatrolRange;
    public float stoppingAngle;
    // Rigging
    public Transform LookAt;
    private MultiAimConstraint multiAim;
    // 动画相关参数
    float Speed;
    bool isAiming;
    public bool isDead; 
    public bool isTurn;
    bool isLeftTurn;
    float Idle = 0;
    float walk = 2;
    float chase = 3;
    public int getHit = 0;
    private bool isPatrol;
    // 运动停止相关参数
    public float BunkerStoppingDistance;
    public float guardStoppingDistance;
    public float ChaseStoppingDistance;

    public float continueTime; // 发现目标的持续时间
    public float ExitTime; // 脱战的持续时间
    public float tiggerTime;
    
    // 音频效果
    public AudioClip[] FootstepAudioClips;
    public GameObject dropItemPrefab;
    // Emoji
    private ExpressionUI expressionUI;
    private Dictionary<string, List<string>> emojiActions;
    private List<string> currentEmoji;

    public BuffManager buffManager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        buffManager = GetComponent<BuffManager>(); 

        InitGameObjects();
    }

    void Start()
    {
        guardPos = transform.position;
        guardRotation = transform.rotation;

        emojiActions = new Dictionary<string, List<string>>()
        {
            {"无", new List<string>{}},
            {"发现", new List<string>{UIImage.感叹号, UIImage.感叹号}},
            {"追击", new List<string>{UIImage.怒}},
            {"死亡", new List<string>{UIImage.哭}},
            {"难绷", new List<string>{UIImage.中指, UIImage.苦笑, UIImage.中指}}
        };
        currentEmoji = emojiActions["无"];

        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
        }
        
        expressionUI = GetComponent<ExpressionControl>().ExpressionPrefab.GetComponent<ExpressionUI>();
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();

    }

    void SwitchAnimation()
    {
        animator.SetBool("Aim", isAiming);
        animator.SetBool("Rifle", isGunToting);
        animator.SetBool("LeftTurn",isLeftTurn);
        animator.SetBool("Turn", isTurn);
        animator.SetFloat("Speed", Speed);
    }

    void SwitchEmoji(List<string> stateActions)
    {
        if (currentEmoji == stateActions)
        {
            return ;
        }
        currentEmoji = stateActions;
        expressionUI.Refresh(stateActions);
    }

    private void InitGameObjects()
    {
        foreach (Transform ChildObj in GetComponentsInChildren<Transform>())
        {
            if (ChildObj.name == "AimToPlayer Rig")
            {
                multiAim = ChildObj.Find("Chest Constraint").GetComponent<MultiAimConstraint>();
            }
        }
    }

    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
            if (getHit >= 4)
            {
                SwitchEmoji(emojiActions["难绷"]);
            }
            else
            {
                SwitchEmoji(emojiActions["死亡"]);
            }
            DestroyObject();
        }
        else if (FoundPlayer(signtRadius) && enemyStates != EnemyStates.BATTLE  && continueTime >= tiggerTime)
        {
            enemyStates = EnemyStates.CHASE;
            
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
            if (continueTime != 0)
            {
                SwitchEmoji(emojiActions["发现"]);
            }
            else
            {
                SwitchEmoji(emojiActions["无"]);
            }
            if (transform.position != guardPos)
            {
                agent.destination = guardPos;
                if (Vector3.SqrMagnitude(guardPos - transform.position) <= guardStoppingDistance)
                {
                    guardPos = transform.position;
                    Turn(guardRotation);
                }
            }
            else
            {
                Speed = Idle;
                agent.speed = Idle;
                
            }
            break;

            case EnemyStates.PATROL:
            Speed = walk;
            agent.speed = walk;

            break;

            case EnemyStates.CHASE:
            SwitchEmoji(emojiActions["追击"]);
            agent.speed = chase;
            Speed = chase;

            // 不在目标区域内且 脱战时间小于触发时间
            if (!FoundPlayer(signtRadius) && ExitTime < tiggerTime)
            {
                ExitTime = Mathf.Clamp(ExitTime + Time.deltaTime, 0f, tiggerTime);
                agent.destination = GameManager.Instance.playerManager.transform.position;
            }
            // 不在目标区域内且 脱战时间等于触发时间 
            if (!FoundPlayer(signtRadius) && ExitTime == tiggerTime)
            {

                if (!isGetGuardPosition)
                {
                    isGetGuardPosition = true;
                    
                    rotationToGuardPosition = Quaternion.LookRotation(guardPos - transform.position);
                }
                Turn(rotationToGuardPosition);
                if (isTurn)
                {
                    return;
                }
                
                if (isGuard && !isTurn)
                {
                    continueTime = 0f;
                    Speed = walk;
                    agent.speed = walk;
                    enemyStates = EnemyStates.GUARD;
                }
                else if (isPatrol && !isTurn)
                {
                    continueTime = 0f;
                    enemyStates = EnemyStates.PATROL;
                }
            }
            else if (FoundPlayer(signtRadius) && continueTime == tiggerTime)
            {
                
                Vector3 direction = attackTarget.transform.position - transform.position;
                agent.destination = attackTarget.transform.position - direction.normalized * (ChaseStoppingDistance - 1);
                Rotate(attackTarget);

                if (Vector3.Magnitude(transform.position - attackTarget.transform.position) <= ChaseStoppingDistance)
                {
                    enemyStates = EnemyStates.BATTLE;
                    
                }

            }
            break;

            case EnemyStates.BATTLE:
                // 超出一定范围退出
                if (FoundPlayer(battleRadius))
                {
                    if (isGunToting)
                    {
                        isAiming = true;
                    }
                    agent.speed = Idle;
                    Speed = Idle;
                    agent.destination = transform.position;
                    // TODO 设置随机移动
                    Rotate(attackTarget);
                    if (FoundBunker())
                    {
                        float destination = 0f;
                        Vector3 targetPoint = attackTarget.transform.position;
                        Vector3[] points = bunker.GetComponent<CalBoundary>().points;
                        for (int i = 0; i < points.Length; i++)
                        {
                            if (Vector3.Distance(points[i], attackTarget.transform.position) > destination)
                            {
                                destination = Vector3.Distance(points[i], attackTarget.transform.position);
                                targetPoint = points[i];
                            }
                        }
                        agent.destination = targetPoint;
                        if (Vector3.SqrMagnitude(targetPoint - transform.position) <= BunkerStoppingDistance)
                        {
                            Speed = Idle;
                            agent.speed = Idle;
                            Quaternion rotationToAttacker = Quaternion.LookRotation(attackTarget.transform.position - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToAttacker, 5f * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    isAiming = false;
                    enemyStates = EnemyStates.CHASE;
                }

            break;
        }
    }

    bool FoundPlayer(float Radius)
    {
        
        var colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                LookAt.position = GameManager.Instance.playerManager.LookPoint.transform.position;
                LookAt.rotation = GameManager.Instance.playerManager.LookPoint.transform.rotation;
                attackTarget = collider.gameObject;
                isTurn = false;
                multiAim.weight = Mathf.Lerp(multiAim.weight, 1f, Time.deltaTime);
                continueTime = Mathf.Clamp(continueTime + Time.deltaTime, 0f, tiggerTime);
                ExitTime = 0f;
                return true;
            }
        }
        continueTime = 0f;
        multiAim.weight =  Mathf.Lerp(multiAim.weight, 0f, Time.deltaTime);
        attackTarget = null;
        return false;
    }

    bool FoundBunker()
    {
        var colliders = Physics.OverlapSphere(transform.position, battleRadius);
        foreach (var collider in colliders)
            {
                if (collider.CompareTag("Bunker"))
                {
                    bunker = collider.gameObject;

                    return true;
                }
            }
        bunker = null;
        return false;
    }

    private void Turn(Quaternion targetRotation)
    {
        if (agent.speed != Idle)
        { 
            Speed = Idle;
            agent.speed = Idle;
            if (!isTurn)
            {
                currentRotation = transform.rotation;
                if (Quaternion.Dot(targetRotation, currentRotation) < 0f)
                {
                    isLeftTurn = false;
                }
                else
                {   
                    isLeftTurn = true;
                }
            }
        }
        
        if (Quaternion.Angle(transform.rotation, targetRotation) >= stoppingAngle)
        {
            isTurn = true;
        }
        else
        {
            isTurn = false;
            isGetGuardPosition = false;
        }
    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
        Vector3 PlayerPosition = GameManager.Instance.playerManager.transform.position;
        Vector3 thisPosition = transform.position;
        float volume = AudioVolumeProcessor.AudioVolumeCalculate(
                thisPosition, 
                PlayerPosition,
                1f, 0f, 10f
                );
        AudioManager.Instance.soundFXManager.PlayRandomSoundFXClip(FootstepAudioClips, transform, volume);
    }

    void PutGrabRifle()
    {
        
    }

    void SetTowHandsWeight()
    {

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, signtRadius);
    }

    public void Rotate(GameObject attackTarget)
    {
        Vector3 direction = attackTarget.transform.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, 5f * Time.deltaTime);
    }

    private void SetRigidBodiesNonKinematic()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    public void DestroyObject()
    {
        animator.enabled = false;
        SetRigidBodiesNonKinematic();
        StartCoroutine(DestroyAfter());
    }

    private IEnumerator DestroyAfter()
    {
        
        yield return new WaitForSeconds(5f);
        Destroy(GetComponent<ExpressionControl>().ExpressionPrefab);
        Destroy(gameObject);
    }

}
