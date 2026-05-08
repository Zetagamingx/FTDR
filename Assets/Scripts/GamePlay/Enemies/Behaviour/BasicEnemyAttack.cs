using System.Collections;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private int damage = 5;

    private float cooldownTimer;
    private bool isAttacking;

    private Transform currentTarget;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public bool CanAttack()
    {
        if (currentTarget == null) return false;
        if (isAttacking) return false;
        if (cooldownTimer > 0f) return false;

        float distance = Vector3.Distance(transform.position, currentTarget.position);
        return distance <= attackRange;
    }

    public void StartAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackDuration);

        Attack();

        cooldownTimer = attackCooldown;
        isAttacking = false;
    }

    void Attack()
    {
        Debug.Log(name + " attacked " + currentTarget.name);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}