using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HexGridManager : MonoBehaviour
{
    public Camera mCamera;
    [SerializeField] private LayerMask hexLayer;
    private HexCell lastHitHex;
    [SerializeField] private Transform hexGroup;
    [SerializeField] private List<HexCell> hexGroupList = new ();
    public GameObject tower;

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
    private void CLickToBuildOrRemove(HexCell hexCell)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (hexCell.state == HexState.Active && !hexCell.hasTower)
            {
                BuildTower(hexCell);
            }
            else
            {
                DestroyTower(hexCell);
            }
        }
    }

    private void BuildTower(HexCell hexCell)
    {
        hexCell.hasTower = true;
        Vector3 spawnPos = hexCell.transform.position;

        hexCell.currentTower = Instantiate(tower, spawnPos, Quaternion.identity);
        hexCell.ChangeHexCellColors();
    }

    private void DestroyTower(HexCell hexCell)
    {
        if (hexCell.currentTower != null)
        {
            Destroy(hexCell.currentTower);
            hexCell.currentTower = null;
        }

        hexCell.hasTower = false;
    }
    public void RemoveAllTowers()
    {
        for (int i = 0; i < hexGroupList.Count; i++)
        {
            if (hexGroupList[i] != null && hexGroupList[i].hasTower && hexGroupList[i].state == HexState.Active)
            {
                DestroyTower(hexGroupList[i]);
                hexGroupList[i].hasTower = false;
                hexGroupList[i].ChangeHexCellColors();
            }
        }
    }

    void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hexLayer))
        {
            if (hit.transform.TryGetComponent<HexCell>(out HexCell currentHex))
            {
                if (currentHex.state == HexState.Active)
                {
                    ClearLastHex();
                    HandleSelection(currentHex);
                    CLickToBuildOrRemove(currentHex);
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
        if (lastHitHex == newHex) return;

        lastHitHex = newHex;
        lastHitHex.isHighlighted = true;
        lastHitHex.ChangeHexCellColors();
    }

    private void ClearLastHex()
    {
        if (lastHitHex != null)
        {
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
