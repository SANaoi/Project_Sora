using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;
public class CharacterShootController : BaseShoot
{
    
    bool isShooting;
    float LastShootTime;
    Animator animator;
    float FireRate;
    private Ray FireRay;
    public GameObject FireHole;
    public PlayerMoveControls InputActions;
    [SerializeField] private AudioClip shootSoundClip;
    private void Awake()
    {
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        LastShootTime = 0f;
        FireRate = 2f;
       
    }
    

    void Update()
    { 
        if (animator.GetBool("Aim"))
        {
            Shoot();
        }
        else
        {
        }
    }

        void Shoot()
    {
        if (Time.time > FireRate + LastShootTime && GetComponent<EnemyController>().attackTarget && animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Aiming Idle"))
        {
            Vector3 shootDirection = -FireHole.transform.forward;
            FireRay = new Ray(FireHole.transform.position, shootDirection);
            AudioManager.Instance.soundFXManager.PlaySoundFXClip(shootSoundClip, transform, GetShootVolume());
            shootDirection.Normalize();

            if (Physics.Raycast(
                    FireRay,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask))
            {
                StartCoroutine(
                PlayTrail(
                FireHole.transform.position,
                hit.point,
                hit));

                 if (hit.transform.CompareTag("Player"))
                    {
                        hit.transform.GetComponent<CharacterStats>().TakeDamage(hit.transform.GetComponent<CharacterStats>());
                    }
            }
            else
            {
                StartCoroutine(
                PlayTrail(
                FireHole.transform.position, 
                FireHole.transform.forward + (shootDirection * TrailConfig.MissDistance), 
                new RaycastHit()));
            }
            LastShootTime = Time.time;
        }
    }

    private float GetShootVolume()
    {
        Vector3 PlayerPosition = GameManager.Instance.playerManager.transform.position;
        Vector3 thisPosition = transform.position;
        return AudioVolumeProcessor.AudioVolumeCalculate(
                thisPosition, 
                PlayerPosition,
                1f, 0f, 10f
                );
    }
}
