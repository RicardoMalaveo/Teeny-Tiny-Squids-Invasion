using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private enum FlightState { Rising, FlyingForward }
    private FlightState currentFlightState = FlightState.Rising;


    private float flightAltitude;
    private float submergedDepth;
    [SerializeField] private float riseSpeedMultiplier;

    private EnemyWayPoints path;
    private int waypointIndex = 0;
    private EnemyInfo info;
    private EnemyDestinyHandler enemyDestinyHandler;
    private Transform castleTarget;

    public void Initialize(EnemyInfo data, Transform target)
    {
        info = data;
        castleTarget = target;
        enemyDestinyHandler = GetComponent<EnemyDestinyHandler>();
        flightAltitude = info.flightAltitude;
        submergedDepth = info.submergedDepth;

        if (info.isAerial)
        {
            transform.position = new Vector3(transform.position.x, submergedDepth, transform.position.z);
            currentFlightState = FlightState.Rising;
        }
        else
        {
            path = Object.FindFirstObjectByType<EnemyWayPoints>();
        }
    }

    void Update()
    {
        if (info.isAerial)
        {
            RiseAndFly();
        }
        else
        {
            MarchingForward();
        }
    }

    private void RiseAndFly()
    {
         Vector3 directionToTarget = castleTarget.position - transform.position;
         directionToTarget.y = 0f;

         if (directionToTarget != Vector3.zero)
         {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
         }

        if (currentFlightState == FlightState.Rising)
        {
            Vector3 targetHeight = new Vector3(transform.position.x, flightAltitude, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetHeight, info.moveSpeed * 2f * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - flightAltitude) < 0.1f)
            {
                currentFlightState = FlightState.FlyingForward;
            }
        }
        else
        {
            Vector3 targetPosition = new Vector3(castleTarget.position.x, flightAltitude, castleTarget.position.z);
            MovementToTarget(targetPosition);

            float distanceToCastle = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                                                      new Vector2(targetPosition.x, targetPosition.z));
            if (distanceToCastle < 1.0f)
            {
                ReachedLastWayPoint();
            }
        }
    }
    private void MarchingForward()
    {
        if (waypointIndex >= path.nodes.Count) return;

        Vector3 targetPosition = path.nodes[waypointIndex].position;
        MovementToTarget(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SwitchToNextWayPoint();
        }
    }
    private void MovementToTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        float movementSpeed = info.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed);
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
            enemyDestinyHandler.ReachCastle();
    }
}
