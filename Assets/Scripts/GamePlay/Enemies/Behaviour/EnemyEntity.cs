using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        LevelManager.Instance.RegisterKill(); // Notify LevelManager of the kill

        Destroy(gameObject);
    }
}