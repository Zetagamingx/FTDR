using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private List<SpawnInNavMeshArea> spawners;

    [SerializeField] private int currentLevelIndex = 0;

    private int totalEnemiesSpawned = 0;
    private int totalEnemiesKilled = 0;

    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        ApplyLevel(currentLevelIndex);
    }

    public LevelData GetLevel(int levelIndex)
    {
        return levels[levelIndex];
    }

    public void ApplyLevel(int levelIndex)
    {
        LevelData level = levels[levelIndex];

        //  RESET COUNTS HERE
        foreach (var category in level.categories)
        {
            foreach (var enemy in category.enemies)
            {
                enemy.currentCount = 0;
            }
        }

        //  THEN assign to spawners
        foreach (var spawner in spawners)
        {
            var category = level.categories.Find(c => c.categoryName == spawner.Category);

            if (category != null)
            {
                spawner.SetEnemies(category.enemies);
            }
        }

        //Debug.Log($"Applied Level {levelIndex}");
    }

    public void RegisterSpawn()
    {
        totalEnemiesSpawned++;
        //Debug.Log($"Enemy Spawned. Total Spawned: {totalEnemiesSpawned}");
    }

    public void RegisterKill()
    {
        totalEnemiesKilled++;
        Debug.Log($"Enemy Killed. Total Killed: {totalEnemiesKilled}");

        if (totalEnemiesKilled >= totalEnemiesSpawned)
        {
            Debug.Log("Level Completed!");
            WaveComplete();//  Here you can trigger level completion logic, such as loading the next level or showing a victory screen.
        }
    }
   
    public void WaveComplete()
    {
        Debug.Log("Level Complete! Proceeding to next wave...");
        //Time.timeScale = 0f; // pause game

        PerkManager.Instance.OfferPerks();
    }
}