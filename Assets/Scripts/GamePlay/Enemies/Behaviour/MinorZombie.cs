using UnityEngine;

public class MinorZombie : EnemyEntity
{
    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("Skeleton Soldier spawned with " + currentHealth + " health.");
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Skeleton Soldier took damage. Current health: " + currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Skeleton Soldier collided with: " + other.gameObject.name);
        if (other.CompareTag("Bullet"))
        {
            int bulletDamage = other.GetComponent<BulletStats>()?.bulletDamage ?? 20; // Get damage from BulletStats, default to 20 if not found
            Debug.Log("Skeleton Soldier hit by Bullet.");

            TakeDamage(bulletDamage); // Example damage value, can be adjusted based on bullet type or other factors
        }
    }
}
