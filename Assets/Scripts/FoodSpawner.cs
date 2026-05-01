using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] healthyPrefabs;
    public GameObject[] unhealthyPrefabs;

    [Header("Spawn Settings")]
    public float spawnRadius = 20f;
    public int maxFood = 25;
    public float spawnInterval = 1f;
    public Transform player;
    public float leftLimit = -13.3f;
public float rightLimit = 1.4f;
private int currentFood = 0;

    [Range(0f, 1f)]
    public float unhealthyChance = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 1f, spawnInterval);
    }

    void SpawnFood()
{
    if (currentFood >= maxFood) return; // 🚫 stop spawning

    bool spawnUnhealthy = Random.value < unhealthyChance;

    GameObject prefab = spawnUnhealthy
        ? unhealthyPrefabs[Random.Range(0, unhealthyPrefabs.Length)]
        : healthyPrefabs[Random.Range(0, healthyPrefabs.Length)];

    Vector3 spawnPos = new Vector3(
        Random.Range(leftLimit, rightLimit),
        30f,
        player.position.z + Random.Range(20f, 50f)
    );

    if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
    {
        Vector3 finalPos = hit.point + Vector3.up * 0.2f;
        GameObject food = Instantiate(prefab, finalPos, Quaternion.identity);

        currentFood++; // ✅ track it
    }
}

}