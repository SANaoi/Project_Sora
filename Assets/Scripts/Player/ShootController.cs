using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class ShootController : BaseShoot
{
    bool isShooting;
    float LastShootTime;
    public int MagazineAmmo;
    public int TotalAmmo;
    Camera mainCamera;
    Animator animator;
    public GameObject FireHole;
    private Ray FireRay;
    // public ShootConfigurationScriptableObject ShootConfig;
    // public TrailConfigScriptableObject TrailConfig;
    // private ObjectPool<TrailRenderer> TrailPool;
    // private ObjectPool<VisualEffect> ImpactPool;
    public PlayerMoveControls InputActions;
    //private Vector3 EndRayPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
    private UIGunInfo GunInfo;
    // public VisualEffect VFX_Flash;
    // public VisualEffectAsset ImpactParticle;
    private void Start()
    {
        InputActions = GameManager.Instance.inputActions;
        mainCamera = FindObjectOfType<Camera>();
        // TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        // ImpactPool = new ObjectPool<VisualEffect>(CreateImpactParticle);
        InputActions.Player.Fire.started += OnFireStarte;
        InputActions.Player.Fire.canceled += OnFireCancel;
        animator = GetComponent<Animator>();
        LastShootTime = 0f;
        MagazineAmmo = ShootConfig.Capacity;

        GunInfo = UIManager.Instance.OpenPanel("GunInfo").GetComponent<UIGunInfo>();
        // 2: 步枪子弹
        TotalAmmo = GameManager.Instance.GetPackageLocalItemsNumById(2);
        
    }


    private void Update()
    {
        if (isShooting && animator.GetBool("Aiming") && MagazineAmmo > 0)
        {
            VFX_Flash.GetComponent<VisualEffect>().gameObject.SetActive(true);
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
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<CharacterStats>().TakeDamage(hit.transform.GetComponent<CharacterStats>());
                    }
                }
            else
                {
                    StartCoroutine(
                        PlayTrail(
                            FireHole.transform.position,
                            mainCamera.transform.forward + (shootDirection * TrailConfig.MissDistance),
                            new RaycastHit()
                        )
                    );
                }
            LastShootTime = Time.time;
            
        }
    }
    // private TrailRenderer CreateTrail()
    // {
    //     GameObject instance = new GameObject("Bullet Trail");

    //     TrailRenderer trail = instance.AddComponent<TrailRenderer>();
    //     trail.material = TrailConfig.Material;
    //     trail.colorGradient = TrailConfig.Color;
    //     trail.widthCurve = TrailConfig.WidthCurve;
    //     trail.time = TrailConfig.Duration;
    //     trail.minVertexDistance = TrailConfig.MinVertexDistance;

    //     trail.emitting = false;
    //     trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    //     return trail;
    // }

    // private VisualEffect CreateImpactParticle()
    // {
    //     GameObject instance = new GameObject("Impact Particle");
    //     VisualEffect impact = instance.AddComponent<VisualEffect>();
    //     impact.visualEffectAsset = ImpactParticle;
    //     return impact;
    // }
    
    // protected virtual IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    // {
    //     TrailRenderer trailInstance = TrailPool.Get();
    //     trailInstance.transform.position = StartPoint;
    //     trailInstance.gameObject.SetActive(true);
    //     yield return null;

    //     trailInstance.emitting = true;

    //     float distance = Vector3.Distance(StartPoint, EndPoint);
    //     float remainingDistance = distance;
    //     while (remainingDistance > 0)
    //     {
    //         trailInstance.transform.position = Vector3.Lerp(
    //             StartPoint,
    //             EndPoint,
    //             Mathf.Clamp01(1 - (remainingDistance / distance))
    //         );
    //         remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime ;

    //         yield return null;
    //     }

    //     trailInstance.transform.position = EndPoint;

    //     if (Hit.collider != null)
    //     {
    //         VisualEffect impactInstance = ImpactPool.Get();
    //         impactInstance.transform.position = EndPoint;
    //         impactInstance.gameObject.SetActive(true);
    //         yield return new WaitForSeconds(0.3f);
    //         impactInstance.gameObject.SetActive(false);
    //         ImpactPool.Release(impactInstance);
    //     }

    //     yield return new WaitForSeconds(TrailConfig.Duration);
    //     yield return null;

    //     trailInstance.emitting = false;
    //     trailInstance.gameObject.SetActive(false);
    //     TrailPool.Release(trailInstance);
    // }

    
}
