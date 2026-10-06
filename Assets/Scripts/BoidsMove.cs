using System.Collections.Generic;
using UnityEngine;

public class BoidsMove : MonoBehaviour
{
    public Vector2 direction;

    public float targetDistance;

    public float speed;
    public float speedModifier;
    public float turnSpeed;

    private FlockSpawner flockSpawner;

    [SerializeField] private bool startSerching = true;

    public List<GameObject> neighbours = new List<GameObject>();

    [SerializeField] private bool drawDebugLines = true;
    [SerializeField] private Color debugColor;

    /*public float alignWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1f;*/


    private void Update()
    {
        if (!drawDebugLines) return;

        foreach (var other in neighbours)
        {
            if (other == null) continue;
            Debug.DrawLine(transform.position, other.transform.position, debugColor);
        }
        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.Euler(0f, 0f, angle - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed * Time.deltaTime);

        }
    }

    private void Start()
    {
        flockSpawner = FindFirstObjectByType<FlockSpawner>();

        debugColor = new Color(Random.value, Random.value, Random.value);
    }

    private void FixedUpdate()
    {
        if (startSerching)
        {
            updateNeighbours();
        }

        Vector2 newPosition = (Vector2)transform.position + direction * speed * speedModifier * Time.fixedDeltaTime;
        transform.position = newPosition;

        direction = (direction + alignment * flockSpawner.alignWeight + cohesion * flockSpawner.cohesionWeight + separation * flockSpawner.separationWeight).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private Vector2 alignment, cohesion, separation;

    private void updateNeighbours()
    {
        neighbours.Clear();

        Vector2 pos = transform.position;
        Vector2 dirSum = Vector2.zero;
        Vector2 posSum = Vector2.zero;
        Vector2 sepSum = Vector2.zero;

        foreach (var other in flockSpawner.boidsSpawned)
        {
            if (other == null || other == gameObject) continue;

            Vector2 offset = (Vector2)other.transform.position - pos;
            float dist = offset.magnitude;
            if (dist > flockSpawner.neighbourRadius) continue;

            neighbours.Add(other);

            dirSum += other.GetComponent<BoidsMove>().direction;
            posSum += (Vector2)other.transform.position;
            sepSum -= offset / Mathf.Max(dist * dist, 0.0001f);
        }

        int count = neighbours.Count;
        if (count == 0)
        {
            alignment = cohesion = separation = Vector2.zero;
            return;
        }

        alignment = (dirSum / count).normalized;
        cohesion = ((posSum / count) - pos).normalized;
        separation = sepSum.normalized;
    }
}
