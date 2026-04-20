using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HexGridManager : MonoBehaviour
{
    public static HexGridManager Instance;
    public Camera mCamera;
    [SerializeField] private LayerMask hexLayer;
    private HexCell lastHitHex;
    [SerializeField] private Transform hexGroup;
    [SerializeField] private List<HexCell> hexGroupList = new ();
    [Header("Tower Library")]
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
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            SetHexCellState();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAllTowers();
        }
        HandleHover();
    }

    public void BuildTowerSelected(int towerIndex, HexCell targetHex)
    {
        selectedTowerIndex = towerIndex;

        if (targetHex != null)
        {
            BuildTower(targetHex);
            targetHex.HideBuildingMenu();
        }
    }

    private void BuildTower(HexCell hexCell)
    {
        TowerData data = towerLibrary[selectedTowerIndex];
        hexCell.hasTower = true;
        hexCell.currentTowerData = data;
        Vector3 spawnPos = hexCell.transform.position;
        hexCell.currentTower = Instantiate(data.towerPrefab, spawnPos, Quaternion.identity);
        hexCell.ChangeHexCellColors();
    }

    public void SellTower(HexCell hexCell)
    {
        if (hexCell.currentTower != null && hexCell.currentTowerData != null)
        {
            //int refund = Mathf.RoundToInt(hex.currentTowerData.cost * 0.5f);
            //GameManager.Instance.AddSand(refund);
            Destroy(hexCell.currentTower);
            hexCell.currentTower = null;
            hexCell.currentTowerData = null;
            hexCell.hasTower = false;
            hexCell.ChangeHexCellColors();
            hexCell.HideBuildingMenu();
        }

        
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
                        HandleSelection(currentHex);
                    }
                }
                else
                {
                    ClearLastHex();
                }
            }
        }
        else
        {
            ClearLastHex();
        }
    }



    private void HandleSelection(HexCell newHex)
    {
        lastHitHex = newHex;

        lastHitHex.isHighlighted = true;
        lastHitHex.ShowBuildingMenu();
        lastHitHex.ChangeHexCellColors();
    }

    private void ClearLastHex()
    {
        if (lastHitHex != null)
        {
            lastHitHex.HideBuildingMenu();
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
}
