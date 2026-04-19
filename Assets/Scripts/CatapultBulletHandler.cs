using UnityEngine;

public class CatapultBulletHandler : MonoBehaviour
{
    private Transform target;
    private Vector3 startPosition;
    public float arcHeight = 5f;
    public float speed = 1.5f; // Duration of flight
    private float progress = 0f;

    public void SetTarget(Transform _target)
    {
        target = _target;
        startPosition = transform.position;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        progress += Time.deltaTime * speed;

        // Linear path to target
        Vector3 currentPos = Vector3.Lerp(startPosition, target.position, progress);

        // Add the Y arc using a Sin wave
        currentPos.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;

        transform.position = currentPos;

        if (progress >= 1f)
        {
            // target.GetComponent<EnemyInfo>().TakeDamage(damage);
            Destroy(target.gameObject);
        }
    }
}
