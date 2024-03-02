using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class ShootController : MonoBehaviour
{
    bool isShooting;
    float LastShootTime;
    int MagazineAmmo;
    int TotalAmmo;
    Camera mainCamera;
    Animator animator;
    public GameObject FireHole;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    private ObjectPool<TrailRenderer> TrailPool;
    private ObjectPool<VisualEffect> ImpactPool;
    public PlayerMoveControls InputActions;
    private Vector3 EndRayPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
    private UIGunInfo GunInfo;
    public VisualEffect VFX_Flash;
    public VisualEffectAsset ImpactParticle;
    private void Start()
    {
        InputActions = PlayerMoveControls.Instance;
        mainCamera = FindObjectOfType<Camera>();
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        ImpactPool = new ObjectPool<VisualEffect>(CreateImpactParticle);
        InputActions.Player.Fire.started += OnFireStarte;
        InputActions.Player.Fire.canceled += OnFireCancel;
        animator = GetComponent<Animator>();
        LastShootTime = 0f;
        MagazineAmmo = ShootConfig.Capacity;

        GunInfo = UIManager.Instance.GetUIGameObject("GunInfo").GetComponent<UIGunInfo>();
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
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");

        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.material = TrailConfig.Material;
        trail.colorGradient = TrailConfig.Color;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;
    }

    private VisualEffect CreateImpactParticle()
    {
        GameObject instance = new GameObject("Impact Particle");
        VisualEffect impact = instance.AddComponent<VisualEffect>();
        impact.visualEffectAsset = ImpactParticle;
        return impact;
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
            // TODO 实现从背包中获取所有子弹数
            GunInfo.Refresh(MagazineAmmo, 120);
            VFX_Flash.Play();
            Vector3 shootDirection = mainCamera.transform.forward
                + new Vector3(
                    Random.Range(
                        -ShootConfig.Spread.x,
                        ShootConfig.Spread.x
                    ),
                    Random.Range(
                        -ShootConfig.Spread.x,
                        ShootConfig.Spread.x
                    ),
                    Random.Range(
                        -ShootConfig.Spread.x,
                        ShootConfig.Spread.x
                    )
                );
            shootDirection.Normalize();


            if (Physics.Raycast(
                    mainCamera.ScreenPointToRay(EndRayPoint),
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
    
    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer trailInstance = TrailPool.Get();
        trailInstance.transform.position = StartPoint;
        trailInstance.gameObject.SetActive(true);
        yield return null;

        trailInstance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            trailInstance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime ;

            yield return null;
        }

        trailInstance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            VisualEffect impactInstance = ImpactPool.Get();
            impactInstance.transform.position = EndPoint;
            impactInstance.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            impactInstance.gameObject.SetActive(false);
            ImpactPool.Release(impactInstance);
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;

        trailInstance.emitting = false;
        trailInstance.gameObject.SetActive(false);
        TrailPool.Release(trailInstance);
    }

    
}
