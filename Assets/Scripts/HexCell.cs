using UnityEngine.UI;
using UnityEngine;
public enum HexState
{
    Active,     // la construccion esta habilitada
    Disabled,   // la construccion no esta habilitada
    Underwater  // para futura implementacion de la marea
}
public class HexCell : MonoBehaviour
{
    public HexState state = HexState.Disabled;
    [SerializeField] private bool isUnderSeaLevel;
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
    public Color underWater;

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
        if (state == HexState.Underwater)
        {
            targetColor = underWater;
        }
        else if (state == HexState.Active)
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
        else // Disabled
        {
            targetColor = disabled;
        }

        sprite1.color = targetColor;
        sprite2.color = targetColor;
    }
}
