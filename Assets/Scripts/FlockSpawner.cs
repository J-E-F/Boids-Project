using UnityEngine;

public class FlockSpawner : MonoBehaviour
{
    private float spawnRange = 10f;

    public GameObject flockPrefab;
    public int flockCount = 10;

    public GameObject[] boidsSpawned;
    private void Start()
    {
        boidsSpawned = new GameObject[flockCount];

        for (int i = 0; i < flockCount; i++)
        {
            boidsSpawned[i] = spawnFlock();
        }
    }

    public GameObject spawnFlock()
    {
        Vector2 randomVectorSpawn = new Vector2(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));

        Quaternion randomZRotation = Quaternion.Euler(0f, 180f, Random.Range(0f, 360f));
        GameObject newBoid = Instantiate(flockPrefab, randomVectorSpawn, randomZRotation);
        return newBoid;
    }
}
