using System.Collections.Generic;
using UnityEngine;
//This is a script that spawns the boids in random positions, and keeps track of all the spawned boids in a list. 
public class FlockSpawner : MonoBehaviour
{
    private float spawnRange = 10f;

    private Vector2 moveForwardSpeed;

    public GameObject flockPrefab;
    [Range(1, 150)] public int flockCount;

    public List<GameObject> boidsSpawned = new List<GameObject>(); //list of all the spawned boids

    [Header("Weights")]
    [Range(0f, 1.5f)] public float alignWeight = 1f;
    [Range(0f, 1.5f)] public float cohesionWeight = 1f;
    [Range(0f, 1.5f)] public float separationWeight = 1f;//all the weights for the boids movement, can be adjusted in the inspector

    public float neighbourRadius = 5f;
    public void Update()
    {
        while (boidsSpawned.Count < flockCount)
        {
            boidsSpawned.Add(spawnFlock());//adds a new boid to the list of spawned boids until the count is equal to the flockCount
        }
        while (boidsSpawned.Count > flockCount)
        {
            int last = boidsSpawned.Count - 1;
            Destroy(boidsSpawned[last]);
            boidsSpawned.RemoveAt(last);//destroys all the boids outside of the list
        }
    }

    public GameObject spawnFlock()
    {
        Vector2 randomVectorSpawn = new Vector2(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));//spawnes the boid in a random position on the screen

        Quaternion randomZRotation = Quaternion.Euler(0f, 0, Random.Range(0f, 360f));
        GameObject newBoid = Instantiate(flockPrefab, randomVectorSpawn, randomZRotation);
        return newBoid;
    }
}
