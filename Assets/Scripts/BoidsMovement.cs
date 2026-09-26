using UnityEngine;
using System.Collections.Generic;

public class BoidsMovement : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10f;

    public int speedMultiplier = 2;
    public float rotationSpeed;

    [SerializeField] private float distanceToTarget;

    public Rigidbody2D rB2D;

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

    private void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        rB2D.linearVelocity = direction * speed * speedMultiplier;
    }

    private void Update()
    {
        findVisibleTargets();

        Vector2 steering = alignment();
        Vector2 newDir = direction + steering * Time.deltaTime;

        direction = (direction + steering * Time.deltaTime).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        rB2D.linearVelocity = direction * speed * speedMultiplier;
    }
    public Vector2 alignment()
    {
        Vector2 avgVelocity = Vector2.zero;
        int total = 0;

        foreach (GameObject boid in visibleBoids)
        {
            if (boid == null || boid == gameObject) continue;

            BoidsMovement other = boid.GetComponent<BoidsMovement>();

            if (other == null || other.rB2D == null) continue;

            float dist = Vector2.Distance(transform.position, boid.transform.position);

            if (dist <= alignmentRadius)
            {
                avgVelocity += other.rB2D.linearVelocity;
                total++;
            }
        }
        if (total >= 0)
        {
            avgVelocity /= total;
            Vector2 steering = avgVelocity - rB2D.linearVelocity;
            return steering;
        }
        return Vector2.zero;
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