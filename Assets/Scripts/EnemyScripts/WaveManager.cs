using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private List<WaveData> levelsWaves;
    private int currentWaveIndex = 0;
    [SerializeField] private List<Transform> groundSpawnPoints;
    [SerializeField] private List<Transform> airSpawnPoints;
    [SerializeField] private Transform castleTarget;

    private int activeEnemiesCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public float GetCurrentWaveDuration()
    {
        if (currentWaveIndex >= levelsWaves.Count) return 0;

        EnemyWayPoints path = Object.FindFirstObjectByType<EnemyWayPoints>();
        float realPathDistance = (path != null) ? path.GetTotalPathDistance() : 50f;

        WaveData wave = levelsWaves[currentWaveIndex];
        float totalTime = 0;
        float slowestSpeed = float.MaxValue;

        foreach (var group in wave.spawnGroups)
        {
            totalTime += (group.count * group.intervalBetweenEnemies);
            if (group.enemyType.moveSpeed < slowestSpeed)
                slowestSpeed = group.enemyType.moveSpeed;
        }
        totalTime += wave.delayBetweenGroups * (wave.spawnGroups.Count - 1);
        float travelTime = realPathDistance / slowestSpeed;

        return totalTime + travelTime;
    }

    public void DecrementEnemyCount()
    {
        activeEnemiesCount--;
    }
    public void StartNextWave()
    {
        if (currentWaveIndex >= levelsWaves.Count) return;
        StartCoroutine(SpawnWaveRoutine(levelsWaves[currentWaveIndex]));
    }

    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        int totalInWave = 0;
        foreach (var group in wave.spawnGroups) totalInWave += group.count;
        activeEnemiesCount = totalInWave;

        foreach (var group in wave.spawnGroups)
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemyType);
                yield return new WaitForSeconds(group.intervalBetweenEnemies);
            }
            yield return new WaitForSeconds(wave.delayBetweenGroups);
        }

        yield return new WaitUntil(() => activeEnemiesCount <= 0);

        EndWave(wave);
    }

    private void SpawnEnemy(EnemyInfo info)
    {
        List<Transform> points = info.isAerial ? airSpawnPoints : groundSpawnPoints;
        Transform selectedPoint = points[Random.Range(0, points.Count)];

        GameObject enemyGO = Instantiate(info.enemyPrefab, selectedPoint.position, selectedPoint.rotation);

        if (enemyGO.TryGetComponent<EnemyDestinyHandler>(out var handler))
        {
            handler.Setup(info, castleTarget);
        }

        if (enemyGO.TryGetComponent<EnemyMovement>(out var mover))
        {
            mover.Initialize(info, castleTarget);
        }
    }

    private void EndWave(WaveData wave)
    {
        float budget = wave.GetTotalWaveBudget();
        int bonus = Mathf.RoundToInt(budget * wave.bonusPercentage);
        GameManager.Instance.AddSand(bonus);

        currentWaveIndex++;
        GameManager.Instance.OnWaveExtinction();
    }
}
