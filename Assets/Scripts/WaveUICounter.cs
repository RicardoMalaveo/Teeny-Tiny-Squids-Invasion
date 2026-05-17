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

    private void Start()
    {
        SetTimelineActive(false);
    }

    public void SetTimelineActive(bool isActive)
    {
        spawningNowText.gameObject.SetActive(isActive);
        comingNextText.gameObject.SetActive(isActive);
    }

    public void UpdateTimeline(string currentName, int currentRemaining, EnemySpawnGroup next)
    {
        if (currentRemaining > 0)
        {
            spawningNowText.text = $"Llegando a la costa:\n{currentName} x {currentRemaining}";
        }
        else
        {
            spawningNowText.text = "Siguientes invasores en llegar:\nMore Alien Scum On the Way";
        }

        if (next != null)
        {
            comingNextText.text = $"Siguientes en llegar:\n{next.enemyType.enemyName} x {next.count}";
        }
        else
        {
            comingNextText.text = "Siguientes en llegar:\nPrepárate para la siguiente invasión!";
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
            totalCompositionText.text = "¡Todos los invasores han sido derrotados!";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<b>Fuerzas Invasoras:</b>");
        var sortedEnemies = liveWaveEnemies.OrderBy(x => x.Key.dangerLevel);

        foreach (var kvp in sortedEnemies)
        {
            sb.AppendLine($"[{kvp.Key.dangerLevel}] {kvp.Key.enemyName} x {kvp.Value}");
        }

        totalCompositionText.text = sb.ToString();
    }
}
