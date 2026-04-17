using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCategory
{
    public string categoryName;
    public List<EnemyEntry> enemies;
}

[System.Serializable]
public class EnemyEntry
{
    public GameObject prefab;
    public int maxCount;

    [HideInInspector] public int currentCount;
}

[CreateAssetMenu(menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber;

    public List<EnemyCategory> categories;
}


