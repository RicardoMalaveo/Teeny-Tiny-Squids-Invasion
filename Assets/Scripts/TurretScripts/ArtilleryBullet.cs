using UnityEngine;

public class ArtilleryBullet : CatapultBullet
{
    [SerializeField] private float blastRadius;

    protected override void HitTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<EnemyDestinyHandler>(out var enemy))
            {
                enemy.ApplyDamage(damage);
            }
        }

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
