using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum EnemyState
    {
        MoveToObjective,
        ChasePlayer,
        Attack,
        Wander,
        Dead
    }

    [Header("References")]
    [SerializeField] private List<GameObject> targets;

    private BasicEnemyMovement movement;
    private BasicEnemyAttack attack;

    private Transform player;
    private Transform currentTarget;

    [Header("Aggro")]
    [SerializeField] private float aggroDuration = 5f;
    [SerializeField] private float playerDetectRange = 10f;

    private float aggroTimer;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 6f;

    private EnemyState currentState;

    void Awake()
    {
        movement = GetComponent<BasicEnemyMovement>();
        attack = GetComponent<BasicEnemyAttack>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        ChangeState(EnemyState.MoveToObjective);

        StartCoroutine(BrainRoutine());
    }

    public void SetTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
    }

    IEnumerator BrainRoutine()
    {
        while (currentState != EnemyState.Dead)
        {
            Think();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void Think()
    {
        aggroTimer -= 0.25f;

        switch (currentState)
        {
            case EnemyState.MoveToObjective:
                HandleMoveToObjective();
                break;

            case EnemyState.ChasePlayer:
                HandleChasePlayer();
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;

            case EnemyState.Wander:
                HandleWander();
                break;
        }
    }

    void HandleMoveToObjective()
    {
        GameObject nearestTarget = GetNearestObjective();

        if (nearestTarget == null) return;

        currentTarget = nearestTarget.transform;

        attack.SetTarget(currentTarget);

        if (attack.CanAttack())
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        movement.MoveTo(currentTarget.position);

        if (CanSeePlayer())
        {
            AggroPlayer();
        }
    }

    void HandleChasePlayer()
    {
        if (player == null)
        {
            ChangeState(EnemyState.MoveToObjective);
            return;
        }

        currentTarget = player;

        attack.SetTarget(currentTarget);

        if (attack.CanAttack())
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        movement.MoveTo(player.position);

        if (aggroTimer <= 0f)
        {
            ChangeState(EnemyState.Wander);
        }
    }

    void HandleAttack()
    {
        movement.StopMoving();

        if (attack.IsAttacking())
            return;

        if (attack.CanAttack())
        {
            attack.StartAttack();
        }
        else
        {
            if (aggroTimer > 0f)
                ChangeState(EnemyState.ChasePlayer);
            else
                ChangeState(EnemyState.MoveToObjective);
        }
    }

    void HandleWander()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        randomPoint.y = transform.position.y;

        movement.MoveTo(randomPoint);

        ChangeState(EnemyState.MoveToObjective);
    }

    void AggroPlayer()
    {
        aggroTimer = aggroDuration;
        ChangeState(EnemyState.ChasePlayer);
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);

        return distance <= playerDetectRange;
    }

    GameObject GetNearestObjective()
    {
        if (targets == null || targets.Count == 0)
            return null;

        GameObject closest = null;
        float bestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }
}