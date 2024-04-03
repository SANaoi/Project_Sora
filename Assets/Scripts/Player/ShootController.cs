using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class ShootController : BaseShoot
{
    public bool isShooting;
    float LastShootTime;

    float accumulatedBallisticOffset_x; // 弹道累计的偏移量
    float accumulatedBallisticOffset_y;

    float targetOffset_x;
    float targetOffset_y;

    public int MagazineAmmo;
    public int TotalAmmo;
    Camera mainCamera;
    Animator animator;
    public GameObject FireHole;
    private Ray FireRay;
    public PlayerMoveControls InputActions;
    private UIGunInfo GunInfo;
    private CinemachinePOV virtualCamera;

    private Cinemachine.CinemachineCollisionImpulseSource Inpulse;
    private void Start()
    {
        InputActions = GameManager.Instance.inputActions;
        mainCamera = FindObjectOfType<Camera>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
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
            
            StartCoroutine(accumulatedOffset());
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
            // Vector3 shootDirection = LookPointObject.transform.position;
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
                        else if (Enemy.GetComponent<CharacterStats>().CurrentHealth <= 0 && enemyController.agent.isStopped == false)
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
    private IEnumerator accumulatedOffset()
    {   

        Vector2 RandomVector2 = new Vector2(
                Random.Range(
                    -ShootConfig.Spread.x,
                    ShootConfig.Spread.x),
                Random.Range(
                0,
                ShootConfig.Spread.y));

        virtualCamera.m_HorizontalAxis.Value -= RandomVector2.x;
        virtualCamera.m_VerticalAxis.Value -= RandomVector2.y;
        yield return null;

    }

}
