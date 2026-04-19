using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    public GameObject enemyPrefab;
    public int count;             
}

[CreateAssetMenu(fileName = "Wave_", menuName = "Spawner Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Composition")]
    public List<EnemySpawnGroup> enemiesToSpawn;

    [Header("Settings")]
    public float timeBetweenSpawns = 1.0f;
    public float delayBeforeNextGroup = 2.0f;
}
