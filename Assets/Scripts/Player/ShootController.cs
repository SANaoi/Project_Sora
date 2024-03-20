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
    public PlayerMoveControls InputActions;
    private UIGunInfo GunInfo;
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
}
