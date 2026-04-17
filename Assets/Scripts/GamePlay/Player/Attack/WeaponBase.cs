using UnityEngine;
using UnityEngine.InputSystem;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected BulletPooling bulletPool;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected InputActionReference fireAction;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float bulletSpeed = 20f;
    [SerializeField] protected float bulletLifetime = 2f;
    
    protected float fireCooldown;

    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (CanFire())
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    protected abstract void Fire();

    protected abstract bool CanFire();
}