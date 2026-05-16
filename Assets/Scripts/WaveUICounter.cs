using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class WaveUICounter : MonoBehaviour
{
    public static WaveUICounter Instance;

    [SerializeField] private TextMeshProUGUI spawningNowText;
    [SerializeField] private TextMeshProUGUI comingNextText;
    [SerializeField] private TextMeshProUGUI totalCompositionText;

    private Dictionary<EnemyInfo, int> liveWaveEnemies = new Dictionary<EnemyInfo, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateTimeline(string currentName, int currentRemaining, EnemySpawnGroup next)
    {
        if (currentRemaining > 0)
        {
            spawningNowText.text = $"Spawning Now:\n{currentName} x {currentRemaining}";
        }
        else
        {
            spawningNowText.text = "Spawning Now:\nNext Enemy Wave In coming";
        }

        if (next != null)
        {
            comingNextText.text = $"Spawning Next:\n{next.enemyType.enemyName} x {next.count}";
        }
        else
        {
            comingNextText.text = "Spawning Next:\nEnd of enemy wave";
        }
    }

    public void SetupTotalWaveComposition(WaveData wave)
    {
        liveWaveEnemies.Clear();

        foreach (var group in wave.spawnGroups)
        {
            if (liveWaveEnemies.ContainsKey(group.enemyType))
            {
                liveWaveEnemies[group.enemyType] += group.count;
            }
            else
            {
                liveWaveEnemies.Add(group.enemyType, group.count);
            }
        }

        RenderTotalCompositionUI();
    }

    public void OnEnemyDefeated(EnemyInfo enemyType)
    {
        if (liveWaveEnemies.ContainsKey(enemyType))
        {
            liveWaveEnemies[enemyType]--;

            if (liveWaveEnemies[enemyType] <= 0)
            {
                liveWaveEnemies.Remove(enemyType);
            }

            RenderTotalCompositionUI();
        }
    }

    private void RenderTotalCompositionUI()
    {
        if (liveWaveEnemies.Count == 0)
        {
            totalCompositionText.text = "All enemies have been defeated!";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<b>Remaining Waves Threat Profile:</b>");
        var sortedEnemies = liveWaveEnemies.OrderBy(x => x.Key.dangerLevel);

        foreach (var kvp in sortedEnemies)
        {
            sb.AppendLine($"[{kvp.Key.dangerLevel}] {kvp.Key.enemyName} x {kvp.Value}");
        }

        totalCompositionText.text = sb.ToString();
    }
}
