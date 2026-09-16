using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class BoidsMovement : MonoBehaviour
{
    [SerializeField]private float speed = 100f;
    [SerializeField]private float rotationSpeed = 5f;

    public bool moveForward = true;
    public bool seperation = false;
    public bool rule2 = false;
    public bool rule3 = false;

    private Vector2 currVelocity;

    public bool BoidInsideFOW = false;

    public Rigidbody2D rB2D;

    [Header("DetectionCone")]
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleBoids = new List<Transform>();
    private void Start()
    {
        rB2D.GetComponent<Rigidbody2D>();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void Update()
    {
        if (moveForward)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        if (seperation)
        {
            moveForward = false;
            separation();
            findVisibleTargets();
        }
    }

    public void separation()
    {
        Vector2 curr = transform.position;
        Vector2 avoidDir = Vector2.zero;
        int tooCloseCount = 0;

        foreach (Transform boid in visibleBoids)
        {
            if (boid == transform) continue;

            Vector2 other = boid.position;
            float dist = Vector2.Distance(curr, other);

            if (dist < viewRadius && dist > 0.0001f)
            {
                Vector2 away = (curr - other) / dist; // closer = stronger influence
                avoidDir += away;
                tooCloseCount++;
            }
        }

        if (tooCloseCount > 0)
        {
            avoidDir /= tooCloseCount;

            float targetAngle = Mathf.Atan2(avoidDir.y, avoidDir.x) * Mathf.Rad2Deg - 90f;

            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }
    public Vector2 Velocity => currVelocity;
    private void findVisibleTargets()
    {
        visibleBoids.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform traget = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (traget.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector2.Distance(transform.position, traget.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleBoids.Add(traget);
                }
            }
        }
    }
}
