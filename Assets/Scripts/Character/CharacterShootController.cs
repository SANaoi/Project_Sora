using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.VFX;
public class CharacterShootController : MonoBehaviour
{
    
    bool isShooting;
    float LastShootTime;
    Animator animator;
    GameObject attackTarget;
    private Ray FireRay;
    public GameObject FireHole;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    private ObjectPool<TrailRenderer> TrailPool;
    private ObjectPool<VisualEffect> ImpactPool;
    public PlayerMoveControls InputActions;
    public VisualEffect VFX_Flash;
    public VisualEffectAsset ImpactParticle;

    private void Awake()
    {
        // attackTarget = GetComponent<EnemyController>().attackTarget;
    }
    private void Start()
    {
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        ImpactPool = new ObjectPool<VisualEffect>(CreateImpactParticle);
        animator = GetComponent<Animator>();
        LastShootTime = 0f;
        isShooting = true;
       
    }

    void Update()
    { 
        if (isShooting && animator.GetBool("Aim"))
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


    void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime && GetComponent<EnemyController>().attackTarget)
        {
            Vector3 shootDirection = -FireHole.transform.forward;
            FireRay = new Ray(FireHole.transform.position, shootDirection);
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

    protected virtual IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
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
