using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int playerHealth;

    public int bonusDamage = 0;

    public static PlayerStats Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterHitDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player takes {damage} damage, health now {playerHealth}");
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Here you can add logic to handle player death, such as triggering animations, restarting the level, etc.
    }

    public int GetFinalDamage(int baseDamage)
    {
        return baseDamage + bonusDamage;
    }
}
