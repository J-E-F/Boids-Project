using System.Collections.Generic;
using UnityEngine;
//boids movment script, handles the movement of the boids based on the alignment, cohesion and separation forces.
public class BoidsMove : MonoBehaviour
{
    public Vector2 direction;
    public float targetDistance;

    [Header("Movement Settings")]
    public float speed;
    public float speedModifier;
    public float turnSpeed;

    private FlockSpawner flockSpawner;

    [SerializeField] private bool startSerching = true;

    public List<GameObject> neighbours = new List<GameObject>();

    [Header("Debug values")]
    [SerializeField] private bool drawDebugLines = true;
    [SerializeField] private Color debugColor;

    [Header("Boids Forces")]
    private Vector2 alignment, cohesion, separation;

    /*public float alignWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1f;*/

    private void Start()
    {
        flockSpawner = FindFirstObjectByType<FlockSpawner>();

        debugColor = new Color(Random.value, Random.value, Random.value);//debug color for each boid, random RGB values
    }
    private void Update()
    {
        if (!drawDebugLines) return;

        foreach (var other in neighbours)
        {
            if (other == null) continue;
            Debug.DrawLine(transform.position, other.transform.position, debugColor);//debug line between this boid and its neighbours
        }
        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.Euler(0f, 0f, angle - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed * Time.deltaTime);//updates the current ransform rotation of the boid based on the new direction and turn speed

        }
    }

    private void FixedUpdate()
    {
        if (startSerching)
        {
            updateNeighbours();
        }

        Vector2 newPosition = (Vector2)transform.position + direction * speed * speedModifier * Time.fixedDeltaTime;
        transform.position = newPosition;

        direction = (direction + alignment  //Calcuclate the new direction based on the alignment, cohesion and separation forces
            * flockSpawner.alignWeight + cohesion 
            * flockSpawner.cohesionWeight + separation 
            * flockSpawner.separationWeight).normalized;

        transform.position += (Vector3)(direction * speed * Time.deltaTime); //updates the current position of the boid based on the new direction and speed
    }

    private void updateNeighbours()
    {
        neighbours.Clear();

        Vector2 pos = transform.position; //this boids current position
        Vector2 neighbourDirSum = Vector2.zero; //neighbours direction sum, set to zero at the start of each update
        Vector2 neighbourPosSum = Vector2.zero;//neighbours position sum, set to zero at the start of each update
        Vector2 seperationSum = Vector2.zero;//neighbours seperation sum, set to zero at the start of each update

        foreach (var other in flockSpawner.boidsSpawned)
        {
            if (other == null || other == gameObject) continue;

            Vector2 offset = (Vector2)other.transform.position - pos;
            float dist = offset.magnitude;//get the distance between this boid and the other boid
            if (dist > flockSpawner.neighbourRadius) continue;

            neighbours.Add(other);

            neighbourDirSum += other.GetComponent<BoidsMove>().direction;//calculate the sum of the directions of all neighbouring boids
            neighbourPosSum += (Vector2)other.transform.position;//calculate the sum of the positions of all neighbouring boids
            seperationSum -= offset / Mathf.Max(dist * dist, 0.0001f);//calculate the sum of the separation forces of all neighbouring boids, inversely proportional to the square of the distance
        }

        int count = neighbours.Count;
        if (count == 0)
        {
            alignment = cohesion = separation = Vector2.zero;//if there are no neighbours, set the alignment, cohesion and separation forces to zero
            return;
        }

        alignment = (neighbourDirSum / count).normalized;//calculate the average direction of the neighbouring boids and normalize it to get the alignment force
        cohesion = ((neighbourPosSum / count) - pos).normalized;//calculate the average position of the neighbouring boids, subtract this boid's position to get the direction towards the centre of mass, and normalize it to get the cohesion force
        separation = seperationSum.normalized;//normalize the separation force to get the final separation force
    }
}
