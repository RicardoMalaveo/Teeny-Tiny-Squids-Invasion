using UnityEngine;

public class CatapultBullet: ProjectileBase
{
    [SerializeField] private float arcHeight = 5f;
    private Vector3 startPosition;
    private float progress = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    protected override void Move()
    {
        progress += Time.deltaTime * (speed / 10f);

        Vector3 currentPos = Vector3.Lerp(startPosition, target.position, progress);


        currentPos.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;

        transform.position = currentPos;

        if (progress >= 1f) HitTarget();
    }
}
