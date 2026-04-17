using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets;

    private NavMeshAgent agent;
    private int currentTargetIndex;

    private BasicEnemyAttack attack;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<BasicEnemyAttack>();
    }

    void Start()
    {
        PickNewTarget();
        StartCoroutine(TargetRoutine());
    }

    public void SetTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
    }

    private void PickNewTarget()
    {
        //Debug.Log($"{name}: Picking new target...");
       
        if (targets == null || targets.Count == 0)
        {
            //Debug.LogWarning($"{name}: No targets assigned.");
            return;
        }

        currentTargetIndex = Random.Range(0, targets.Count);
        Vector3 destination = targets[currentTargetIndex].transform.position;

        agent.SetDestination(destination);
    }

    private IEnumerator TargetRoutine()
    {
        while (true)
        {

            if (attack != null && attack.IsAttacking())
            {
                agent.isStopped = true;
                yield return null;
                continue;
            }
            else
            {
                agent.isStopped = false;
            }

            // Only check when path is ready
            if (!agent.pathPending)
            {
                // Check if reached destination
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Optional: small delay before switching target
                    yield return new WaitForSeconds(1f);

                    PickNewTarget();
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}