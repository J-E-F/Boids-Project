using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlockSpawner : MonoBehaviour
{
    private float spawnRange = 10f;

    private Vector2 moveForwardSpeed;

    public GameObject flockPrefab;
    [Range(1, 150)] public int flockCount;

    public List<GameObject> boidsSpawned = new List<GameObject>();

    [Header("Weights")]
    [Range(0f, 1.5f)] public float alignWeight = 1f;
    [Range(0f, 1.5f)] public float cohesionWeight = 1f;
    [Range(0f, 1.5f)] public float separationWeight = 1f;

    public float neighbourRadius = 5f;
    public void Update()
    {
        while (boidsSpawned.Count < flockCount)
        {
            boidsSpawned.Add(spawnFlock());
        }
        while (boidsSpawned.Count > flockCount)
        {
            int last = boidsSpawned.Count - 1;
            Destroy(boidsSpawned[last]);
            boidsSpawned.RemoveAt(last);
        }
    }

    public GameObject spawnFlock()
    {
        Vector2 randomVectorSpawn = new Vector2(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));

        Quaternion randomZRotation = Quaternion.Euler(0f, 0, Random.Range(0f, 360f));
        GameObject newBoid = Instantiate(flockPrefab, randomVectorSpawn, randomZRotation);
        return newBoid;
    }
}
