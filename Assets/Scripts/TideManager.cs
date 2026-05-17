using System.Collections.Generic;
using UnityEngine;

public class TideManager : MonoBehaviour
{
    public static TideManager Instance;
    private List<HexCell> currentlyFloodedCells = new List<HexCell>();


    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void ExecuteImmediateFloodAndSell()
    {
        HexGridManager.Instance.ForceClearHoverAndMenus();
        List<HexCell> allHexes = HexGridManager.Instance.hexGroupList;

        currentlyFloodedCells.Clear();

        foreach (var cell in allHexes)
        {
            if (cell != null && cell.isUnderSeaLevel)
            {
                cell.underWater = true;
                cell.isUnderSeaLevel = false;

                if (cell.hasTower)
                {
                    HexGridManager.Instance.SellTower(cell);
                }
                else
                {
                    cell.ChangeHexCellColors();
                }

                currentlyFloodedCells.Add(cell);
            }
        }
    }

    public void ForecastNextWaveTide(WaveData nextWaveData)
    {
        List<HexCell> allHexes = HexGridManager.Instance.hexGroupList;

        foreach (var cell in allHexes)
        {
            if (cell != null && cell.state == HexState.Active && cell.isUnderSeaLevel)
            {
                cell.isUnderSeaLevel = false;
                cell.ChangeHexCellColors();
            }
        }

        if (nextWaveData == null || !nextWaveData.startTideWarning)
        {
            return;
        }

        List<HexCell> activeCandidates = new List<HexCell>(allHexes);
        if (activeCandidates.Count == 0) return;

        float intensity = Mathf.Clamp01(nextWaveData.floodIntensity);
        int targetCount = Mathf.RoundToInt(activeCandidates.Count * intensity);
        targetCount = Mathf.Clamp(targetCount, 1, activeCandidates.Count);

        for (int i = 0; i < activeCandidates.Count; i++)
        {
            HexCell temp = activeCandidates[i];
            int randomIndex = Random.Range(i, activeCandidates.Count);
            activeCandidates[i] = activeCandidates[randomIndex];
            activeCandidates[randomIndex] = temp;
        }

        for (int i = 0; i < targetCount; i++)
        {
            activeCandidates[i].isUnderSeaLevel = true;
            activeCandidates[i].ChangeHexCellColors();
        }
    }

    public void ForceResetEntireGrid()
    {
        List<HexCell> allHexes = HexGridManager.Instance.hexGroupList;
        ClearFloodedCells();

        foreach (var cell in allHexes)
        {
            if (cell != null)
            {
                cell.isUnderSeaLevel = false;
                cell.underWater = false;
                cell.ChangeHexCellColors();
            }
        }
    }

    private void ClearFloodedCells()
    {
        if (currentlyFloodedCells.Count == 0) return;

        foreach (var cell in currentlyFloodedCells)
        {
            if (cell != null)
            {
                cell.underWater = false;
                cell.ChangeHexCellColors();
            }
        }
        currentlyFloodedCells.Clear();
    }
}
