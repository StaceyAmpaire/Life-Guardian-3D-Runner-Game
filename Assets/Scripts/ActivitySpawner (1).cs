using UnityEngine;

public class ActivitySpawner : MonoBehaviour
{
    [Header("Activity Prefabs")]
    public GameObject[] activityHealthyPrefabs;
    public GameObject[] activityUnhealthyPrefabs;

    [Header("Spawn Settings")]
    public float spawnRadius = 20f;
    public int maxItems = 25; // Renamed from maxFood
    public float spawnInterval = 1f;
    public Transform player;
    public float leftLimit = -13.3f;
    public float rightLimit = 1.4f;
    private int currentItems = 0; // Renamed from currentFood

    [Range(0f, 1f)]
    public float unhealthyChance = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnActivity), 1f, spawnInterval);
    }

    void SpawnActivity()
    {
        if (currentItems >= maxItems) return; // 🚫 stop spawning

        bool spawnUnhealthy = Random.value < unhealthyChance;
        GameObject prefabToSpawn = null;

        prefabToSpawn = spawnUnhealthy
            ? activityUnhealthyPrefabs[Random.Range(0, activityUnhealthyPrefabs.Length)]
            : activityHealthyPrefabs[Random.Range(0, activityHealthyPrefabs.Length)];
        
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No activity prefabs assigned. Cannot spawn item.");
            return;
        }

        Vector3 spawnPos = new Vector3(
            Random.Range(leftLimit, rightLimit),
            30f,
            player.position.z + Random.Range(20f, 50f)
        );

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
        {
            Vector3 finalPos = hit.point + Vector3.up * 0.2f;
            GameObject item = Instantiate(prefabToSpawn, finalPos, Quaternion.identity); // Renamed from food

            currentItems++; // ✅ track it
        }
    }
}
