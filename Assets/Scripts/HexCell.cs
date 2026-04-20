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
    public TowerData currentTowerData;
    private GameObject spawnedMenu;
    [Header("UI Interaction")]
    public GameObject hexCellBuildTowerMenuPrefab;
    public GameObject hexCellManageTowerMenuPrefab;
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
        if (spawnedMenu != null) return;

        GameObject prefabToSpawn = hasTower ? hexCellManageTowerMenuPrefab : hexCellBuildTowerMenuPrefab;

        Vector3 uiPos = transform.position + Vector3.up * 0.1f;
        spawnedMenu = Instantiate(prefabToSpawn, uiPos, Quaternion.identity);

        GameObject uiCamObj = GameObject.Find("UICamera");
        if (uiCamObj != null)
        {
            Camera uiCam = uiCamObj.GetComponent<Camera>();
            Canvas canvas = spawnedMenu.GetComponent<Canvas>();

            canvas.worldCamera = uiCam;
        }
        spawnedMenu.transform.LookAt(spawnedMenu.transform.position + Camera.main.transform.forward);

        BuildingTowerButtons(spawnedMenu);
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

        if (!hasTower)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => {
                    HexGridManager.Instance.BuildTowerSelected(index, this);
                });
            }
        }
        else
        {
            if (buttons.Length > 0)
            {
                buttons[0].onClick.AddListener(() => {
                    HexGridManager.Instance.SellTower(this);
                });
            }
        }
    }
}
