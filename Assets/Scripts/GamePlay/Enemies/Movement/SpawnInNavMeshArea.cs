using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Configuration;
using UnityEngine;
using UnityEngine.AI;

public class SpawnInNavMeshArea : MonoBehaviour
{
    [SerializeField] private string category;
    [SerializeField] private TargetProvider targetProvider;
    [SerializeField] private LevelManager levelManager;

    private List<EnemyEntry> enemyPool;

    public float radius = 5f;
    public int maxAttempts = 20;

    public string Category => category;

    int spawnAreaMask;

    void Awake()
    {
        int areaIndex = NavMesh.GetAreaFromName("SpawnZone");
        spawnAreaMask = 1 << areaIndex;
    }

    public void SetEnemies(List<EnemyEntry> enemies)
    {
        enemyPool = enemies;
    }

    IEnumerator Start()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(5f);
        }
    }

    public void Spawn()
    {
        if (enemyPool == null || enemyPool.Count == 0)
        {
            Debug.LogWarning("No enemies assigned to spawner!");
            return;
        }

        //Debug.Log("Spawn called");


        for (int i = 0; i < maxAttempts; i++)
        {

            if (enemyPool == null || enemyPool.Count == 0)
            {
                Debug.LogWarning("No enemies assigned to spawner!");
                return;
            }

            List<EnemyEntry> validEnemies = enemyPool.FindAll(e => e.currentCount < e.maxCount);
            
            if (validEnemies.Count == 0)
            {
                //Debug.Log("All enemies reached max count for this spawner.");
                return;
            }

            Vector3 randomPoint = transform.position + Random.insideUnitSphere * radius;

            //Debug.Log("Trying position...");

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5f, spawnAreaMask))
            {
                int index = Random.Range(0, validEnemies.Count);
                EnemyEntry selected = validEnemies[index];

                GameObject enemy = Instantiate(selected.prefab, hit.position + Vector3.up * 0.5f, Quaternion.identity);

                LevelManager.Instance.RegisterSpawn();

                var movement = enemy.GetComponent<BasicEnemyMovement>();
                if (movement != null)
                {
                    movement.SetTargets(targetProvider.targets);
                }

                //Debug.Log("Spawned at " + hit.position);

                selected.currentCount++;

                return;
            }
        }

        Debug.LogWarning("Failed to find valid spawn position in SpawnZone");
    }
}