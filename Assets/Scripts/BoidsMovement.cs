using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Hierarchy;

public class BoidsMovement : MonoBehaviour
{
    public Vector2 direction;
    public Vector2 speed;

    public int speedMultiplier = 2;
    public float rotationSpeed;

    [SerializeField]private float distanceToTarget;

    public Rigidbody2D rB2D;

    [Header("DetectionCone")]
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
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
        rB2D.GetComponent<Rigidbody2D>();
        direction = Random.insideUnitCircle.normalized;
        speed = new Vector2(Random.Range(3, 6), Random.Range(3, 6)) * speedMultiplier;
        //transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        rB2D.linearVelocity = speed * direction * speedMultiplier;
    }

    private void Update()
    {
        findVisibleTargets();
        alignment();
        rB2D.linearVelocity = speed * direction * speed;

    }
    public void alignment()
    {
        Vector2 steering = new Vector2(0,0);
        int total = 0;

        foreach (GameObject boid in visibleBoids)
        {
            if (boid == null) continue;

            BoidsMovement boidVelcoity = boid.GetComponent<BoidsMovement>();

            //if (boidVelcoity == null || boidVelcoity.rB2D == null) continue;

            if (boid != gameObject && distanceToTarget <= 2f)
            {
                steering += boidVelcoity.rB2D.linearVelocity;
                total++;
            }
        }
        if (total >= 0)
        {
            steering /= total;
            steering = speed*direction*speedMultiplier;
            steering -= rB2D.linearVelocity;

            //rB2D.linearVelocity += steering;
        }
        //return steering;
    }


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
                distanceToTarget = Vector2.Distance(transform.position, traget.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleBoids.Add(targetsInViewRadius[i].gameObject);
                }
            }
        }
    }
}
