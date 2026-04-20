using UnityEngine;

public class BaseTurretBulletHandler : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    private float damage;

    public void SetTarget(Transform _target)
    {
        target = _target; 
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }


    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target.TryGetComponent<EnemyDestinyHandler>(out var enemyDestinyHandler))
            {
                enemyDestinyHandler.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
