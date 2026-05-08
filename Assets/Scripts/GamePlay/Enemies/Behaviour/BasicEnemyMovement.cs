using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent == null) return;

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void StopMoving()
    {
        if (agent == null) return;

        agent.isStopped = true;
    }

    public bool ReachedDestination()
    {
        if (agent.pathPending) return false;

        return agent.remainingDistance <= agent.stoppingDistance;
    }
}