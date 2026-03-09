using UnityEngine;
public enum HexState
{
    Active,     // la construccion esta habilitada
    Disabled    // la construccion no esta habilitada
}
public class HexCell : MonoBehaviour
{
    public HexState state = HexState.Disabled;
    public bool hasTower = false;
    public bool isHighlighted;
    public GameObject currentTower;

    [SerializeField] private SpriteRenderer sprite1;
    [SerializeField] private SpriteRenderer sprite2;

    public Color active;
    public Color occupied;
    public Color selected;
    public Color disabled;

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
        if(state == HexState.Active)
        {
            if(!hasTower)
            {
                if(isHighlighted)
                {
                    targetColor = selected;
                }
                else
                {
                    targetColor = active;
                }

            }
            else
            {
                targetColor = occupied;
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
