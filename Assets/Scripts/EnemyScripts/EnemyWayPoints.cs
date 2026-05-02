using UnityEngine;
using System.Collections.Generic;

public class EnemyWayPoints : MonoBehaviour
{
    public Color LineColor;
    [Range(0, 1)] public float SphereRadius;
    public List<Transform> nodes = new ();

    public void OnDrawGizmos()
    {
        Gizmos.color = LineColor;

        nodes.Clear();
        foreach (Transform child in transform)
        {
            nodes.Add(child);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentWaypoint = nodes[i].position;
            Gizmos.DrawSphere(currentWaypoint, SphereRadius);

            if (i > 0)
            {
                Vector3 previousWaypoint = nodes[i - 1].position;
                Gizmos.DrawLine(previousWaypoint, currentWaypoint);
            }
        }
    }
}
