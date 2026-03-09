using UnityEngine;
using System.Collections.Generic;

public class aiMovementManager : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    private EnemyWayPoints enemyWayPoints;
    public List<Transform> nodes = new List<Transform>();
    public Transform currentWayPoint;
    [Range(0, 10)] public int distanceOffset;
    [Range(0, 10)] public float rotationSpeed;
    public int AIcurrentNode;
    public int nodeCheck = 0;


    private void Awake()
    {
        enemyWayPoints = GameObject.FindGameObjectWithTag("Path").GetComponent<EnemyWayPoints>();
        nodes = enemyWayPoints.nodes;
    }
    private void FixedUpdate()
    {
        AIDrive();
        WayPointDistanceCalculator();
    }

    private void AIDrive()
    {
        vertical = 0.8F;
        AIRotation();
    }
    private void AIRotation()
    {
        //horizontal enemy rotation
        Vector3 relative = transform.InverseTransformPoint(currentWayPoint.transform.position);
        relative /= relative.magnitude;


        horizontal = (relative.x / relative.magnitude) * rotationSpeed;
    }
    private void WayPointDistanceCalculator()
    {
        try
        {
            Vector3 position = gameObject.transform.position;
            float distance = Mathf.Infinity;

            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 difference = nodes[i].transform.position - position;
                float currentDistance = difference.magnitude;
                if (currentDistance < distance)
                {
                    if ((i + distanceOffset) >= nodes.Count)
                    {
                        currentWayPoint = nodes[1];
                        distance = currentDistance;
                    }
                    else
                    {
                        currentWayPoint = nodes[i + distanceOffset];
                        distance = currentDistance;
                    }
                    AIcurrentNode = i;
                }
            }
        }
        catch
        {
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentWayPoint.position, 1);
    }
}
