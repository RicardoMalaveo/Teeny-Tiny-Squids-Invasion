using UnityEngine.UI;
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
    private GameObject spawnedMenu;
    [Header("UI Interaction")]
    public GameObject hexCellBuildMenuPrefab; 
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

    public void ShowBuildingMenu()
    {
        if (spawnedMenu == null && !hasTower)
        {
            Vector3 uiPos = transform.position + Vector3.up * 0.1f;
            Vector3 dirToCamera = (Camera.main.transform.position - uiPos).normalized;
            uiPos += dirToCamera * 0.5f;
            spawnedMenu = Instantiate(hexCellBuildMenuPrefab, uiPos, Quaternion.identity);
            spawnedMenu.transform.LookAt(spawnedMenu.transform.position + Camera.main.transform.forward);


            BuildingTowerButtons(spawnedMenu);
        }
    }

    public void HideBuildingMenu()
    {
        if (spawnedMenu != null)
        {
            Destroy(spawnedMenu);
        }
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

    private void BuildingTowerButtons(GameObject menu)
    {
        Button[] buttons = menu.GetComponentsInChildren<Button>();

        if (buttons.Length >= 1)
        {
            buttons[0].onClick.AddListener(() => {
                HexGridManager.Instance.BuildTowerFromUI(0, this);
            });
        }

        if (buttons.Length >= 2)
        {
            buttons[1].onClick.AddListener(() => {
                HexGridManager.Instance.BuildTowerFromUI(1, this);
            });
        }
    }
}
