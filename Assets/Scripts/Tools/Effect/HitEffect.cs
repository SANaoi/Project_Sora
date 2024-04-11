using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    public static float hitForce = 5f;
    public static float shakeDuration = 0.5f;
    public static float shakeStrength = 0.5f;
    public static int vibration = 10;
    public static bool useRigidbody = true;

    public static void OnHit(Rigidbody rb, Vector3 hitDirection)
    {
        //if (rb != null)
        //{
        //    rb.AddForce(- hitDirection * hitForce, ForceMode.Impulse);
        //}
        rb.transform.DOShakePosition(shakeDuration, shakeStrength, vibration, 90f, false, true)
                 .SetEase(Ease.OutElastic);
    }
}
