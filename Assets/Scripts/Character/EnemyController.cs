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
    private GameObject attackTarget;

    [Header("Basic Settings")]
    public float signtRadius;
    public bool isGuard;
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
        animator.SetBool("LeftTurn",isLeftTurn);
        animator.SetBool("Turn", isTurn);
        animator.SetFloat("Speed", Speed);
    }

    void SwitchStates()
    {
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
                isWalk = true;
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
            isChase = false;
            isWalk = true;
            Speed = walk;
            agent.speed = walk;

            break;

            case EnemyStates.CHASE:
            isWalk = false;
            isChase = true;
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
                
                if (isGuard)
                {
                    // transform.rotation = Quaternion.Slerp(transform.rotation, guardRotation, 5f * Time.deltaTime); 
                    Speed = walk;
                    agent.speed = walk;
                    enemyStates = EnemyStates.GUARD;
                }
                else
                {
                    enemyStates = EnemyStates.PATROL;
                }
            }
            else
            {
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
                
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    private void Turn(Quaternion targetRotation)
    {
        if (agent.speed != Idle)
        { 
            Speed = Idle;
            agent.speed = Idle;
        }

        if (!isTurn)
        {
            currentRotation = transform.rotation;
            if (Quaternion.Dot(targetRotation, currentRotation) < 0f)
            {
                isLeftTurn = true;
            }
            else
            {   
                isLeftTurn = false;
            }
        }

        if (Quaternion.Angle(transform.rotation, targetRotation) >= stoppingAngle)
        {
            isTurn = true;
        }
        else
        {
            isTurn = false;
            isLeftTurn = false;
            isGetGuardPosition = false;
        }
    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
        
    }
}
