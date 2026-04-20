using UnityEngine;

public class CatapultBulletHandler : MonoBehaviour
{
    private Transform target;
    private Vector3 startPosition;
    public float arcHeight = 5f;
    public float speed = 1.5f;
    private float progress = 0f;
    private float damage;

    public void SetTarget(Transform _target)
    {
        target = _target;
        startPosition = transform.position;
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        progress += Time.deltaTime * speed;
        Vector3 currentPos = Vector3.Lerp(startPosition, target.position, progress);
        currentPos.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;
        transform.position = currentPos;

        if (progress >= 1f)
        {
            if (target.TryGetComponent<EnemyDestinyHandler>(out var handler))
            {
                handler.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
