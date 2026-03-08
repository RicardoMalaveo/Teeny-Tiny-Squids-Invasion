using UnityEngine;

public class HexInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject towerPrefab;
    public Material highlightMaterial;

    private Material originalMaterial;
    private GameObject lastHoveredHex;
    private Renderer lastRenderer;

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Hexagon"))
            {
                GameObject currentHex = hit.collider.gameObject;

                if (lastHoveredHex != currentHex)
                {
                    ResetLastHex();

                    lastHoveredHex = currentHex;
                    lastRenderer = currentHex.GetComponent<Renderer>();
                    originalMaterial = lastRenderer.material;

                    lastRenderer.material = highlightMaterial;
                }
            }
            else
            {
                ResetLastHex();
            }
        }
        else
        {
            ResetLastHex();
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && lastHoveredHex != null)
        {
            Vector3 spawnPos = lastHoveredHex.transform.position + new Vector3(0, 0.5f, 0);
            Instantiate(towerPrefab, spawnPos, Quaternion.identity);

            Debug.Log("Torre creada en: " + lastHoveredHex.name);
        }
    }

    void ResetLastHex()
    {
        if (lastHoveredHex != null)
        {
            lastRenderer.material = originalMaterial;
            lastHoveredHex = null;
            lastRenderer = null;
        }
    }
}