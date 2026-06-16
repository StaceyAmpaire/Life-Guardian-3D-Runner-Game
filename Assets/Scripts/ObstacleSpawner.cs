using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public Transform[] spawnPoints;

    [Range(0f, 1f)]
    public float spawnChance = 0.7f;

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.value > spawnChance)
                continue;

            int randomIndex =
                Random.Range(0, obstaclePrefabs.Length);

            Instantiate(
                obstaclePrefabs[randomIndex],
                point.position,
                point.rotation,
                transform
            );
        }
    }
}