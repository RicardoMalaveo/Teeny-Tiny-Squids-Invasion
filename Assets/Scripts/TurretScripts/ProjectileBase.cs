using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Transform target;
    protected float damage;
    [SerializeField] protected float speed = 20f;

    public virtual void Setup(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Move();
    }

    protected virtual void Move()
    {
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget()
    {
        if (target != null)
        {
            if (target.TryGetComponent<EnemyDestinyHandler>(out var destiny))
            {
                destiny.ApplyDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}