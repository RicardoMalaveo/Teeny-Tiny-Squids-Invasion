using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    public EnemyInfo enemyType;
    public int count;
    public float intervalBetweenEnemies = 0.5f;

    public float GetGroupCost() => enemyType.dangerLevel * count;
}

[CreateAssetMenu(fileName = "Wave_", menuName = "Spawner Wave Data")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public List<EnemySpawnGroup> spawnGroups;
    public float delayBetweenGroups;
    public float bonusPercentage;

    public float GetTotalWaveBudget()
    {
        float total = 0;
        foreach (var group in spawnGroups) total += group.GetGroupCost();
        return total;
    }
}
