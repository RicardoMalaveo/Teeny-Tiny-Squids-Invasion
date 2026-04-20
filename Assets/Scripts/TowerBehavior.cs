using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    [Header("Turret and Targeting settings")]
    public float range = 15f;
    public float turretRotationSpeed = 10f;
    public Transform headOfTurret;
    private string enemyTag = "Enemy";

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform bulletspawn;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    private Transform target;
    public TowerData towerData;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0F, 0.5F);
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target!= null)
        {
            TurretRotation();
            FiringHandler();
        }
    }

    void TurretRotation()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(headOfTurret.rotation, lookRotation, Time.deltaTime * turretRotationSpeed).eulerAngles;
        headOfTurret.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void FiringHandler()
    {
        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject towerProjectile = Instantiate(projectilePrefab, bulletspawn.position, bulletspawn.rotation);


        if (towerProjectile.TryGetComponent<BaseTurretBulletHandler>(out var baseTurretBulletHandler))
        {
            baseTurretBulletHandler.SetTarget(target);
            baseTurretBulletHandler.SetDamage(towerData.towerDamage);
        }
        else if (towerProjectile.TryGetComponent<CatapultBulletHandler>(out var catapultBulletHandler))
        {
            catapultBulletHandler.SetTarget(target);
            catapultBulletHandler.SetDamage(towerData.towerDamage);
        }
    }
}
