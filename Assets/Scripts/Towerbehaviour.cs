using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private Transform target;
    public float range = 50F;
    private string enemyTag = "Enemy";
    public Transform headOfTurret;
    public Transform barrel;
    public float turretRotationSpeed = 20F;
    public GameObject bulletPrefab;
    public float fireRate = 1F;
    private float fireCountDown = 0F;
    public float launchSpeed = 30F;
    public Transform bulletspawn;

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

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotatio = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotatio.eulerAngles;
        headOfTurret.rotation = Quaternion.Euler(0, rotation.y, 0);

        //fire rate
        if (fireCountDown <= 0F)
        {
            Shoot();
            fireCountDown = 1F / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Shoot()
    {
        Vector3 SpawnPosition = bulletspawn.transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        Vector3 localXDirection = bulletspawn.transform.TransformDirection(Vector3.forward);

        Vector3 velocity = localXDirection * launchSpeed;

        GameObject newObject = Instantiate(bulletPrefab, SpawnPosition, spawnRotation);

        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        rb.linearVelocity = velocity;
    }
}
