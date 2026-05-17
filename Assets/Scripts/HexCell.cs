using UnityEngine.UI;
using UnityEngine;
public enum HexState
{
    Active,     // la construccion esta habilitada
    Disabled   // la construccion no esta habilitad
}
public class HexCell : MonoBehaviour
{
    public HexState state = HexState.Disabled;
    public bool isUnderSeaLevel;
    public bool underWater;
    public bool hasTower = false;
    public bool isHighlighted;
    public GameObject currentTower;
    public TowerData currentTowerData;
    [SerializeField] private SpriteRenderer sprite1;
    [SerializeField] private SpriteRenderer sprite2;

    public Color active;
    public Color occupied;
    public Color selected;
    public Color disabled;
    public Color underSeaLevel;
    public Color occupiedWarning;
    public Color underWaterColor;

    private void Awake()
    {
        ChangeHexCellColors();
    }

    public void UpdateHexCellState(HexState newState)
    {
        state = newState;
        ChangeHexCellColors();
    }

    public void ChangeHexCellColors()
    {
        Color targetColor;

        if (state == HexState.Disabled)
        {
            targetColor = disabled;
        }
        else if (state == HexState.Active || underWater)
        {
            if (underWater)
            {
                targetColor = underWaterColor;
            }
            else if (isUnderSeaLevel)
            {
                if (!hasTower)
                {
                    targetColor = isHighlighted ? selected : underSeaLevel;
                }
                else
                {
                    targetColor = occupiedWarning;
                }
            }
            else
            {
                if (!hasTower)
                {
                    targetColor = isHighlighted ? selected : active;
                }
                else
                {
                    targetColor = occupied;
                }
            }
        }
        else
        {
            targetColor = disabled;
        }

        sprite1.color = targetColor;
        sprite2.color = targetColor;
    }
}