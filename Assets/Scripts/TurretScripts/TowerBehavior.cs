using UnityEngine;
using System.Collections.Generic;

public class TowerBehavior : MonoBehaviour
{
    public TowerData towerData;
    private SphereCollider rangeTrigger;
    private List<EnemyDestinyHandler> targetsInRange = new List<EnemyDestinyHandler>();
    private Transform currentTarget;

    public Transform headOfTurret;
    public Transform bulletSpawn;
    private float fireCountDown = 0f;
    public string shootSoundName;

    private void Start()
    {
        SetupRangeTrigger();
    }

    private void SetupRangeTrigger()
    {
        rangeTrigger = GetComponent<SphereCollider>();
        rangeTrigger.isTrigger = true;
        rangeTrigger.radius = towerData.attackRange;
    }

    void Update()
    {
        UpdateTarget();

        if (currentTarget != null)
        {
            TurretHeadRotation();
            Shooting();
        }
    }

    void UpdateTarget()
    {
        targetsInRange.RemoveAll(enemy => enemy == null);

        if (targetsInRange.Count > 0)
        {
            currentTarget = targetsInRange[0].transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    void TurretHeadRotation()
    {
        Vector3 dir = currentTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(headOfTurret.rotation, lookRotation, Time.deltaTime * towerData.rotationSpeed).eulerAngles;
        headOfTurret.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shooting()
    {
        if (fireCountDown <= 0f)
        {
            Shoot();
            PlaySound();
            fireCountDown = 1f / towerData.fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject projectileGO = Instantiate(towerData.projectilePrefab, bulletSpawn.position, bulletSpawn.rotation);

        if (projectileGO.TryGetComponent<ProjectileBase>(out var projectile))
        {
            projectile.Setup(currentTarget, towerData.towerDamage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyDestinyHandler>(out var enemy))
        {
            if (!targetsInRange.Contains(enemy))
            {
                targetsInRange.Add(enemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyDestinyHandler>(out var enemy))
        {
            targetsInRange.Remove(enemy);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);
    }

    void PlaySound()
    {
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlaySFX(shootSoundName);
        }
    }
}