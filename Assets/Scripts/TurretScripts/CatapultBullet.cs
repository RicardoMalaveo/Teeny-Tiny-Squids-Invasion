using UnityEngine;

public class CatapultBullet : ProjectileBase
{
    [SerializeField] protected float arcHeight;
    [SerializeField] protected float speedWeight;
    protected Vector3 startPosition;
    protected Vector3 targetLandingPoint;
    protected float progress;

    public override void Setup(Transform _target, int _damage)
    {
        base.Setup(_target, _damage);
        startPosition = transform.position;

        if (_target != null)
            targetLandingPoint = _target.position;
    }

    protected override void Move()
    {
        progress += Time.deltaTime * (speed / speedWeight);
        Vector3 currentPos = Vector3.Lerp(startPosition, targetLandingPoint, progress);
        currentPos.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;
        transform.position = currentPos;

        if (progress < 1.0f)
        {
            transform.LookAt(currentPos + (targetLandingPoint - startPosition).normalized);
        }

        if (progress >= 1f) HitTarget();
    }
}
