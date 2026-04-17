using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OfferPerks()
    {
        Debug.Log("Offering perks...");

        // For now: auto-apply one perk (no UI yet)
        ApplyDamagePerk(20);
    }

    void ApplyDamagePerk(int amount)
    {
        PlayerStats.Instance.bonusDamage += amount;

        Debug.Log($"Damage increased by {amount}. Total bonus: {PlayerStats.Instance.bonusDamage}");
    }
}