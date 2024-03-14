using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public enum EnemyStates { GUARD, PATROL, CHASE,BATTLE, DEAD }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour
{

    private NavMeshAgent agent;
    private EnemyStates enemyStates;
    private Animator animator;
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
    bool isDead;
    bool isTurn;
    bool isLeftTurn;
    float Idle = 0;
    float walk = 2;
    float chase = 4;
    private bool isPatrol;
    // 运动停止相关参数
    public float BunkerStoppingDistance;
    public float guardStoppingDistance;
    public float ChaseStoppingDistance;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        InitGameObjects();
    }

    void Start()
    {

        guardPos = transform.position;
        guardRotation = transform.rotation;


        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
        SwitchAnimation();
        LookAt.position = PlayerManager.Instance.LookPoint.transform.position;
        LookAt.rotation = PlayerManager.Instance.LookPoint.transform.rotation;
    }

    void SwitchAnimation()
    {
        animator.SetBool("Aim", isAiming);
        animator.SetBool("Rifle", isGunToting);
        animator.SetBool("LeftTurn",isLeftTurn);
        animator.SetBool("Turn", isTurn);
        animator.SetFloat("Speed", Speed);
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
        }
        else if (FoundPlayer(signtRadius) && enemyStates != EnemyStates.BATTLE)
        {
            enemyStates = EnemyStates.CHASE;
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
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
            agent.speed = chase;
            Speed = chase;
            if (!FoundPlayer(signtRadius))
            {
                if (!isGetGuardPosition)
                {
                    isGetGuardPosition =true;
                    
                    rotationToGuardPosition = Quaternion.LookRotation(guardPos - transform.position);
                }
                Turn(rotationToGuardPosition);
                if (isTurn)
                {
                    return;
                }
                
                if (isGuard && !isTurn)
                {
                    Speed = walk;
                    agent.speed = walk;
                    enemyStates = EnemyStates.GUARD;
                }
                else if (isPatrol && !isTurn)
                {
                    enemyStates = EnemyStates.PATROL;
                }
            }
            else
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
                attackTarget = collider.gameObject;
                isTurn = false;
                multiAim.weight = 1f;
                return true;
            }
        }
        multiAim.weight = 0f;
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
        
    }

    void PutGrabRifle()
    {
        
    }

    void SetTowHandsWeight()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, signtRadius);
    }

    void Rotate(GameObject attackTarget)
    {
        Vector3 direction = attackTarget.transform.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, 5f * Time.deltaTime);

    }
}
