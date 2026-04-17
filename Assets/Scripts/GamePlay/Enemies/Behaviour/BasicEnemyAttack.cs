using System.Collections;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private int damage;

    private float cooldownTimer;
    private Transform player;
    private PlayerStats playerStats; // or whatever your script is called
    private bool isAttacking;

    void Start()
    {
        // Find player once (simple and fine for now)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (player == null || playerStats == null)
            return;

        cooldownTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && cooldownTimer <= 0f && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
            cooldownTimer = attackCooldown;
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Simulate attack duration (tweak this)
        yield return new WaitForSeconds(attackDuration);

        Attack();

        isAttacking = false;
    }

       
    public bool IsAttacking()
    {
        return isAttacking;
    }
    void Attack()
    {
        Debug.Log($"{name} attacks player");

        playerStats.RegisterHitDamage(damage);
    }
}