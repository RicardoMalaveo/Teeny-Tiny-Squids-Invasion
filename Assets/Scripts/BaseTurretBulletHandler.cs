using UnityEngine;

public class BaseTurretBulletHandler : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public float damage = 10f;

    public void SetTarget(Transform _target) => target = _target;

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(target.gameObject);
        }
    }
}
