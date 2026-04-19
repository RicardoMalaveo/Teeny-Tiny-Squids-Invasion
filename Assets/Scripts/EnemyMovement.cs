using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyWayPoints path;
    private int waypointIndex = 0;
    private EnemyInfo info;

    void Start()
    {
        info = GetComponent<EnemyInfo>();
        path = Object.FindFirstObjectByType<EnemyWayPoints>();
    }

    void Update()
    {
        if (path == null || waypointIndex >= path.nodes.Count) return;

       MovementToWayPoint();
    }

    private void MovementToWayPoint()
    {
        Vector3 targetPosition = path.nodes[waypointIndex].position;
        Vector3 direction = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        float movementSpeed = info.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SwitchToNextWayPoint();
        }
    }

    private void SwitchToNextWayPoint()
    {
        if (waypointIndex < path.nodes.Count - 1)
        {
            waypointIndex++;
        }
        else
        {
            ReachedLastWayPoint();
        }
    }

    private void ReachedLastWayPoint()
    {
        Destroy(gameObject);
    }
}
