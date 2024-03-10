using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }

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
    public bool isGuard;
    public bool isGunToting;
    private float lastAttackTime;
    [Header("Patrol State")]
    public float PatrolRange;
    public float stoppingAngle;
    // 动画相关参数
    float Speed;
    bool isWalk;
    bool isChase;
    bool isJumping;
    bool isDead;
    bool isTurn;
    bool isLeftTurn;
    float Idle = 0;
    float walk = 2;
    float chase = 4;
    private bool isPatrol;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        guardPos = transform.position;
        guardRotation = transform.rotation;
    }

    void Start()
    {
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
    }

    void SwitchAnimation()
    {
        animator.SetBool("Rifle", isGunToting);
        animator.SetBool("LeftTurn",isLeftTurn);
        animator.SetBool("Turn", isTurn);
        animator.SetFloat("Speed", Speed);
    }

    void SwitchStates()
    {
        // print(enemyStates);
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
            if (transform.position != guardPos)
            {
                agent.destination = guardPos;
                if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
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
            if (!FoundPlayer())
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
                    if (Vector3.SqrMagnitude(targetPoint - transform.position) <= agent.stoppingDistance)
                    {
                        Speed = Idle;
                        agent.speed = Idle;
                        Quaternion rotationToAttacker = Quaternion.LookRotation(attackTarget.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToAttacker, 5f * Time.deltaTime);
                    }
                    return;
                }
                agent.destination = attackTarget.transform.position;
                Quaternion rotationToTarget = Quaternion.LookRotation(attackTarget.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, 5f * Time.deltaTime); 

            }
            
            break;
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, signtRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                isTurn = false;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    bool FoundBunker()
    {
        var colliders = Physics.OverlapSphere(transform.position, signtRadius);
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
}
