using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class ShootController : BaseShoot
{
    public bool isShooting;
    private bool isShootingActive;
    float LastShootTime;

    float elapsedTime = 0f; // 弹道偏移持续时间
    float durationTime = 0.2f; // 弹道偏移最大持续时间

    public int MagazineAmmo;
    public int TotalAmmo;
    Camera mainCamera;
    Animator animator;
    public GameObject FireHole;
    private Vector3 TrailStartPoint;
    private Ray FireRay;
    private CinemachinePOV virtualCamera;
    private VisualEffect flash;
    private Cinemachine.CinemachineCollisionImpulseSource Inpulse;

    [SerializeField] private AudioClip shootSoundClip;
    private void Awake()
    {
        TotalAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2); // 2: 步枪子弹Id
        flash = CreateImpactFlash(FireHole.gameObject);
        
        MagazineAmmo = ShootConfig.Capacity;

    }

    private void OnEnable() 
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        LastShootTime = 0f;
        Inpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
        mainCamera = FindObjectOfType<Camera>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
        flash.gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {

    }

    private void Update()
    {
        if (isShooting && animator.GetBool("Aiming") && MagazineAmmo > 0 && animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Aiming Idle"))
        {
            Shoot();
            if (isShootingActive || elapsedTime < durationTime)
            {   
                StartCoroutine(AccumulatedOffset());
            }
            flash.gameObject.SetActive(true);
        }
        else
        {
            flash.gameObject.SetActive(false);
        }
        animator.SetBool("isShoot", isShooting);
    }

    public void OnFireStarte(InputAction.CallbackContext context)
    {
        isShooting = true;
    }
    public void OnFireCancel(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    public void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            isShootingActive = true;
            elapsedTime = 0f;
            MagazineAmmo -= 1;
            TrailStartPoint = FireHole.transform.position;
            UIManager.Instance.OpenPanel("GunInfo").GetComponent<UIGunInfo>().Refresh(MagazineAmmo, TotalAmmo);
            flash.Play();
            AudioManager.Instance.soundFXManager.PlaySoundFXClip(shootSoundClip, transform, 1f);
            Vector3 shootDirection = mainCamera.transform.forward;
            
            Inpulse.GenerateImpulse();
            FireRay = new Ray(mainCamera.transform.position, shootDirection);
            shootDirection.Normalize();
            
            if (Physics.Raycast(
                    FireRay,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask))
                {
                    StartCoroutine(
                        PlayTrail(
                            TrailStartPoint,
                            hit.point,
                            hit
                        )
                    );

                    if (hit.transform.GetComponent<Rigidbody>() != null)
                    {
                        Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                        HitEffect.OnHit(rb, hit.transform.forward);
                    }

                    Transform hitInfo = FindRootTransform(hit.transform);
                    if (hitInfo.CompareTag("Enemy"))
                    {
                        
                        
                        Transform Enemy = hitInfo.transform;
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
                            enemyController.gameObject.GetComponent<Collider>().enabled = false;
                            Instantiate(enemyController.dropItemPrefab, Enemy.position, Quaternion.identity);
                        }
                        
                    }
                }
            else
                {
                    StartCoroutine(
                        PlayTrail(
                            TrailStartPoint,
                            mainCamera.transform.position + (shootDirection * TrailConfig.MissDistance),
                            new RaycastHit()
                        )
                    );
                }
            LastShootTime = Time.time;
            
        }
        else
        {
            isShootingActive = false;
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
    // private IEnumerator AccumulatedOffset()
    // {   

    //     float elapsedTime = 0f;
    //     float durationTime = 0.2f;

    //     Vector2 RandomVector2 = new Vector2(
    //             Random.Range(
    //                 -ShootConfig.Spread.x,
    //                 ShootConfig.Spread.x),
    //             Random.Range(
    //             0,
    //             ShootConfig.Spread.y));

    //     float init_x = virtualCamera.m_HorizontalAxis.Value;
    //     float target_x = virtualCamera.m_HorizontalAxis.Value + RandomVector2.x;
    //     float init_y = virtualCamera.m_VerticalAxis.Value;
    //     float target_y = virtualCamera.m_VerticalAxis.Value + RandomVector2.y;

    //     while (elapsedTime < durationTime)
    //     {
    //         // virtualCamera.m_HorizontalAxis.Value = RandomVector2.x;
    //         // virtualCamera.m_VerticalAxis.Value = RandomVector2.y;
    //         virtualCamera.m_HorizontalAxis.Value = Mathf.Lerp(init_x, target_x, elapsedTime / durationTime);
    //         virtualCamera.m_VerticalAxis.Value = Mathf.Lerp(init_y, target_y, elapsedTime / durationTime);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     virtualCamera.m_HorizontalAxis.Value = target_x;
    //     virtualCamera.m_VerticalAxis.Value = target_y;
    // }
    private IEnumerator AccumulatedOffset()
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
        elapsedTime += Time.deltaTime;
    }
}
