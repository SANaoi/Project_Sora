using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class BaseShoot : MonoBehaviour 
{
    public VisualEffect VFX_Flash;
    protected ObjectPool<TrailRenderer> TrailPool;
    protected ObjectPool<VisualEffect> ImpactPool;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    public VisualEffectAsset ImpactParticle;

    public BaseShoot()
    {
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        ImpactPool = new ObjectPool<VisualEffect>(CreateImpactParticle);
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

    protected TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        // instance.AddComponent<GunHit>();
        // instance.AddComponent<SphereCollider>();
        // instance.GetComponent<SphereCollider>().radius = 0.1f;
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

    protected VisualEffect CreateImpactParticle()
    {
        GameObject instance = new GameObject("Impact Particle");
        VisualEffect impact = instance.AddComponent<VisualEffect>();
        impact.visualEffectAsset = ImpactParticle;
        return impact;
    }
}