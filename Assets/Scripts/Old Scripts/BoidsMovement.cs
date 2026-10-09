using UnityEngine;
using System.Collections.Generic;

public class BoidsMovement : MonoBehaviour
{
    public Vector2 direction;

    [Header("DetectionCone")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float alignmentRadius = 2f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<GameObject> visibleBoids = new List<GameObject>();

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void findVisibleTargets()
    {
        visibleBoids.Clear();
        Collider2D[] targetsInViewRadius =
            Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        foreach (var col in targetsInViewRadius)
        {
            Transform target = col.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(direction, dirToTarget) < viewAngle / 2)
            {
                float dist = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dist, obstacleMask))
                {
                    visibleBoids.Add(col.gameObject);
                }
            }
        }
    }
}