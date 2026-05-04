using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HexGridManager : MonoBehaviour
{
    public Camera uiCamera;
    [SerializeField] private float floatingBuildingMenuHight;
    [SerializeField] private GameObject hexBuildMenuPanel;
    [SerializeField] private GameObject hexChangeMenu;
    [SerializeField] private GameObject attackRangeCircle;
    [SerializeField] private float attackRangeCircleElevation;


    public static HexGridManager Instance;
    [SerializeField] private LayerMask hexLayer;
    private HexCell lastHitHex;
    [SerializeField] private Transform hexGroup;
    [SerializeField] private List<HexCell> hexGroupList = new ();


    public List<TowerData> towerLibrary;
    private int selectedTowerIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SyncList();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            SetHexCellState();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RemoveAllTowers();
        }
        HandleHover();
    }

    public void BuildTowerSelected(int towerIndex, HexCell targetHex)
    {
        TowerData data = towerLibrary[towerIndex];

        if (GameManager.Instance.TryPurchase(data.cost))
        {
            BuildTower(targetHex, data);
        }
    }

    private void BuildTower(HexCell hexCell, TowerData data)
    {
        hexCell.hasTower = true;
        hexCell.currentTowerData = data;

        Vector3 spawnPos = hexCell.transform.position;
        hexCell.currentTower = Instantiate(data.towerPrefab, spawnPos, Quaternion.identity);

        hexCell.ChangeHexCellColors();
    }

    public void SellTower(HexCell hexCell)
    {
        float refundMultiplier = GameManager.Instance.refundPercentage;
        int refundAmount = Mathf.RoundToInt(hexCell.currentTowerData.cost * refundMultiplier);
        GameManager.Instance.AddSand(refundAmount);
        Destroy(hexCell.currentTower);

        hexCell.currentTower = null;
        hexCell.currentTowerData = null;
        hexCell.hasTower = false;

        hexCell.ChangeHexCellColors();
    }
    public void RemoveAllTowers()
    {
        for (int i = 0; i < hexGroupList.Count; i++)
        {
            if (hexGroupList[i] != null && hexGroupList[i].hasTower && hexGroupList[i].state == HexState.Active)
            {
                SellTower(hexGroupList[i]);
                hexGroupList[i].hasTower = false;
                hexGroupList[i].ChangeHexCellColors();
            }
        }
    }

    void HandleHover()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hexLayer))
        {
            if (hit.transform.TryGetComponent<HexCell>(out HexCell currentHex))
            {
                if (currentHex.state == HexState.Active)
                {
                    if (currentHex != lastHitHex)
                    {
                        ClearLastHex();
                        HexSelection(currentHex);
                    }
                    return;
                }
            }
        }
        ClearLastHex();
    }

    private void HexSelection(HexCell newHex)
    {
        lastHitHex = newHex;

        lastHitHex.isHighlighted = true;
        FloatingBuildingMenu(lastHitHex);
        lastHitHex.ChangeHexCellColors();
    }

    private void ClearLastHex()
    {
        if (lastHitHex != null)
        {
            HideBuildingMenu();
            lastHitHex.isHighlighted = false;
            lastHitHex.ChangeHexCellColors();
            lastHitHex = null;
        }
    }

    private void SetHexCellState()
    {
        int count = hexGroupList.Count;
        for (int i = 0; i < count; i++)
        {
            if (hexGroupList[i] != null)
            {
                if (hexGroupList[i].state == HexState.Disabled)
                {
                    hexGroupList[i].UpdateHexCellState(HexState.Active);
                }
                else
                {
                    hexGroupList[i].UpdateHexCellState(HexState.Disabled);
                }
            }
        }
    }

    private void FloatingBuildingMenu(HexCell cell)
    {
        hexBuildMenuPanel.SetActive(false);
        hexChangeMenu.SetActive(false);
        GameObject activePanel = cell.hasTower ? hexChangeMenu : hexBuildMenuPanel;
        Vector3 screenPos = cell.transform.position + Vector3.up * floatingBuildingMenuHight;
        activePanel.transform.position = screenPos;
        activePanel.transform.LookAt(activePanel.transform.position + uiCamera.transform.forward);
        activePanel.SetActive(true);
    }


    public void HideBuildingMenu()
    {
        hexBuildMenuPanel.SetActive(false);
        hexChangeMenu.SetActive(false);
    }
    private void OnValidate()
    {
        if (hexGroup != null)
        {
            SyncList();
        }
    }
    public void SyncList()
    {
        hexGroupList.Clear();
        HexCell[] found = hexGroup.GetComponentsInChildren<HexCell>();
        hexGroupList.AddRange(found);
    }

    public void AttackRangeIndicator(TowerData data)
    {
        attackRangeCircle.SetActive(true);
        attackRangeCircle.transform.localScale = Vector3.one;
        float diameter = data.attackRange;
        attackRangeCircle.transform.localScale = new Vector3(diameter, diameter, 1f);
        attackRangeCircle.transform.position = new Vector3(lastHitHex.transform.position.x, attackRangeCircleElevation, lastHitHex.transform.position.z);
    }

    public void HideAttackRangeIndicator()
    {
        attackRangeCircle.SetActive(false);
        ClearLastHex();
    }

    public void OnBuildButtonPressed(int index)
    {
            BuildTowerSelected(index, lastHitHex);
    }

    public void OnSellButtonPressed()
    {
            SellTower(lastHitHex);
            ClearLastHex();
    }
}
