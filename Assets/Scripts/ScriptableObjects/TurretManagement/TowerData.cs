using UnityEngine;

[CreateAssetMenu(fileName = "TowerData_", menuName = "TowerData")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public GameObject towerPrefab;
    public GameObject previewTowerPrefab;

    public int cost;
    public float towerDamage;
    public float fireRate;
    public float attackRange;
    public float rotationSpeed;

    public GameObject projectilePrefab;
}
