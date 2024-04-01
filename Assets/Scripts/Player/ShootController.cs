using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class ShootController : BaseShoot
{
    public bool isShooting;
    float LastShootTime;
    public int MagazineAmmo;
    public int TotalAmmo;
    Camera mainCamera;
    Animator animator;
    public GameObject FireHole;
    private Ray FireRay;
    public PlayerMoveControls InputActions;
    private UIGunInfo GunInfo;

    private Cinemachine.CinemachineCollisionImpulseSource Inpulse;
    private void Start()
    {
        InputActions = GameManager.Instance.inputActions;
        mainCamera = FindObjectOfType<Camera>();
        InputActions.Player.Fire.started += OnFireStarte;
        InputActions.Player.Fire.canceled += OnFireCancel;
        animator = GetComponent<Animator>();
        LastShootTime = 0f;
        MagazineAmmo = ShootConfig.Capacity;

        GunInfo = UIManager.Instance.OpenPanel("GunInfo").GetComponent<UIGunInfo>();
        // 2: 步枪子弹Id
        TotalAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2);
        Inpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
        
    }


    private void Update()
    {
        if (isShooting && animator.GetBool("Aiming") && MagazineAmmo > 0 && animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Aiming Idle"))
        {
            VFX_Flash.GetComponent<VisualEffect>().gameObject.SetActive(true);
            Inpulse.GenerateImpulse();
            
            Shoot();
        }
        else
        {
            VFX_Flash.GetComponent<VisualEffect>().gameObject.SetActive(false);
        }
    }

    private void OnFireStarte(InputAction.CallbackContext context)
    {
        animator.SetBool("isShoot", true);
        isShooting = true;
    }
    private void OnFireCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("isShoot", false);
        isShooting = false;
    }

    public void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
        
            MagazineAmmo -= 1;
            GunInfo.Refresh(MagazineAmmo, TotalAmmo);
            VFX_Flash.Play();
            
            Vector3 shootDirection = mainCamera.transform.forward;
            // Vector3 shootDirection = FireHole.transform.forward;
            FireRay = new Ray(mainCamera.transform.position, shootDirection);
            shootDirection.Normalize();
            
            
            if (Physics.Raycast(
                    // mainCamera.ScreenPointToRay(EndRayPoint),
                    FireRay,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask))
                {
                    StartCoroutine(
                        PlayTrail(
                            FireHole.transform.position,
                            hit.point,
                            hit
                        )
                    );
                    Transform hitInfo = FindRootTransform(hit.transform);
                    if (hitInfo.CompareTag("Enemy"))
                    {
                        Transform Enemy = hitInfo.transform;
                        //CharacterStats characterStats = Enemy.GetComponent<CharacterStats>();
                        EnemyController enemyController = Enemy.GetComponent<EnemyController>();
                        Enemy.GetComponent<CharacterStats>().TakeDamage(Enemy.GetComponent<CharacterStats>());
                        if (Enemy.GetComponent<CharacterStats>().CurrentHealth > 0)
                        {
                            enemyController.enemyStates = EnemyStates.CHASE;
                            enemyController.Rotate(transform.gameObject);
                            enemyController.ExitTime = 0f;
                            enemyController.isTurn = false;
                            enemyController.agent.destination = transform.position;
                        }
                        else
                        {
                            enemyController.agent.isStopped = true;
                            GameObject dropItem = Instantiate(enemyController.dropItemPrefab, Enemy.position, Quaternion.identity);
                        }
                    }
                }
            else
                {
                    StartCoroutine(
                        PlayTrail(
                            FireHole.transform.position,
                            mainCamera.transform.position + (shootDirection * TrailConfig.MissDistance),
                            new RaycastHit()
                        )
                    );
                }
            LastShootTime = Time.time;
            
        }
    }

    private Transform FindRootTransform(Transform obj)
    {   
        while (obj.parent != null)
        {
            obj = obj.parent;
        }
        return obj;
    }
}
