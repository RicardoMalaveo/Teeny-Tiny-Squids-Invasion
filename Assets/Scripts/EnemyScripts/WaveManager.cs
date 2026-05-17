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

    private void Start()
    {
        if (levelsWaves.Count > 0 && TideManager.Instance != null)
        {
            TideManager.Instance.ForecastNextWaveTide(levelsWaves[0]);
            GameManager.Instance.UpdateTideStatusUI(GetCurrentWaveData());
        }
        UpdatePrepUI();
    }
    public WaveData GetCurrentWaveData()
    {
        if (currentWaveIndex < levelsWaves.Count)
        {
            return levelsWaves[currentWaveIndex];
        }
        return null;
    }
    private void UpdatePrepUI()
    {
        if (currentWaveIndex < levelsWaves.Count)
        {
            WaveUICounter.Instance.SetupTotalWaveComposition(levelsWaves[currentWaveIndex]);
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

    public void DecrementEnemyCount(EnemyInfo info)
    {
        activeEnemiesCount--;
        WaveUICounter.Instance.OnEnemyDefeated(info);
    }
    public void StartNextWave()
    {
        if (currentWaveIndex >= levelsWaves.Count) return;
        StartCoroutine(SpawnWaveRoutine(levelsWaves[currentWaveIndex]));
    }

    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        WaveUICounter.Instance.SetTimelineActive(true);

        int totalInWave = 0;
        foreach (var group in wave.spawnGroups) totalInWave += group.count;
        activeEnemiesCount = totalInWave;
        WaveUICounter.Instance.SetupTotalWaveComposition(wave);

        for (int g = 0; g < wave.spawnGroups.Count; g++)
        {
            var currentGroup = wave.spawnGroups[g];
            var nextGroup = (g + 1 < wave.spawnGroups.Count) ? wave.spawnGroups[g + 1] : null;

            for (int i = 0; i < currentGroup.count; i++)
            {
                SpawnEnemy(currentGroup.enemyType);
                int remainingInGroup = currentGroup.count - (i + 1);
                WaveUICounter.Instance.UpdateTimeline(currentGroup.enemyType.enemyName, remainingInGroup, nextGroup);

                yield return new WaitForSeconds(currentGroup.intervalBetweenEnemies);
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

        if (currentWaveIndex >= levelsWaves.Count)
        {
            WaveUICounter.Instance.SetTimelineActive(false);
            GameManager.Instance.WinGame();
            return;
        }

        WaveData upcomingWave = GetCurrentWaveData();
        GameManager.Instance.UpdateTideStatusUI(upcomingWave);
        if (upcomingWave.executeHighTide)
        {
            TideManager.Instance.ExecuteImmediateFloodAndSell();
        }
        else if (upcomingWave.startTideWarning)
        {
            TideManager.Instance.ForecastNextWaveTide(upcomingWave);
        }
        else
        {
            TideManager.Instance.ForceResetEntireGrid();
        }

        WaveUICounter.Instance.SetTimelineActive(false);
        UpdatePrepUI();
        GameManager.Instance.EnemyWaveOver();
    }
}
