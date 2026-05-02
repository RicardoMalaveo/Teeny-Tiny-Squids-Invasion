using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> allWaves;
    private int currentWaveIndex = 0;
    public Transform spawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
            StartCoroutine(SpawnWaveRoutine(allWaves[currentWaveIndex]));
            currentWaveIndex++;
    }

    IEnumerator SpawnWaveRoutine(WaveData wave)
    {

        foreach (EnemySpawnGroup group in wave.enemiesToSpawn)
        {
            for (int i = 0; i < group.count; i++)
            {
                Instantiate(group.enemyPrefab, spawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }

            yield return new WaitForSeconds(wave.delayBeforeNextGroup);
        }
    }
}
