using UnityEngine;

public class BulletStats : MonoBehaviour
{
    [SerializeField] protected int damage = 20;

    public int bulletDamage => damage;

    public void SetDamage(int value)
    {
        damage = value;
    }
}
