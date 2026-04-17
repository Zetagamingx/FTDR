using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowThrowerWeapon : WeaponBase
{
    [SerializeField] private int baseDamage = 20;
    protected override bool CanFire()
    {
        return fireAction.action.IsPressed() && fireCooldown <= 0f;
    }

    protected override void Fire()
    {
        BulletController bullet = bulletPool.Get();
        if (bullet == null) return;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        int finalDamage = PlayerStats.Instance.GetFinalDamage(baseDamage);

        BulletStats stats = bullet.GetComponent<BulletStats>();
        stats.SetDamage(finalDamage);

        //Debug.Log("Before SetActive: " + bullet.gameObject.activeSelf);

        bullet.gameObject.SetActive(true);

        //Debug.Log("After SetActive: " + bullet.gameObject.activeSelf);
        

        bullet.Init(firePoint.forward, bulletSpeed, bulletLifetime);
    }
}