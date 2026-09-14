using UnityEngine;
using System.Collections.Generic;

public class BoidsMovement : MonoBehaviour
{
    [SerializeField]private float speed = 100f;
    public bool moveForward = true;
    public bool seperation = false;
    public bool rule2 = false;
    public bool rule3 = false;

    public bool BoidInsideFOW = false;

    [Header("DetectionCone")]
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

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
        if (seperation)
        {
            moveForward = false;
            findVisibleTargets();
            separation();
        }
    }

    public void separation()
    {
        if (visibleTargets.Count <= 1)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            return;
        }
        else
        {
            Vector2 moveDirection = Vector2.zero;

            foreach (Transform target in visibleTargets)
            {
                if (target != transform)
                {
                    Vector2 dirToTarget = (transform.position - target.position).normalized;
                    moveDirection += dirToTarget;

                    transform.Translate(moveDirection * speed * Time.deltaTime);
                }

            }
        }
    }
    private void findVisibleTargets()
    {
        visibleTargets.Clear();
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
                    visibleTargets.Add(traget);
                }
            }
        }
    }
}
