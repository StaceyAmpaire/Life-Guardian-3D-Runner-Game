using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] healthyPrefabs;
    public GameObject[] unhealthyPrefabs;

    [Header("Spawn Settings")]
    public int maxFood = 25;
    public float spawnInterval = 1f;
    public Transform player;

    [Header("Lane Settings (Match PlayerMovement)")]
    public float laneDistance = 7f; // Must match PlayerMovement laneDistance
    public float centerOffset = -6.0f; // Must match PlayerMovement centerOffset

    private int currentFood = 0;

    [Range(0f, 1f)]
    public float unhealthyChance = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 1f, spawnInterval);
    }

    void SpawnFood()
    {
        if (currentFood >= maxFood) return;

        bool spawnUnhealthy = Random.value < unhealthyChance;

        GameObject prefab = spawnUnhealthy
            ? unhealthyPrefabs[Random.Range(0, unhealthyPrefabs.Length)]
            : healthyPrefabs[Random.Range(0, healthyPrefabs.Length)];

        // --- NEW LANE LOGIC ---
        // Pick a random lane: 0 (Left), 1 (Center), or 2 (Right)
        int randomLane = Random.Range(0, 3);
        
        // Calculate the exact X position for that lane
        float spawnX = (randomLane - 1) * laneDistance + centerOffset;

        Vector3 spawnPos = new Vector3(
            spawnX,
            30f, // High above the ground
            player.position.z + Random.Range(30f, 60f) // Ahead of the player
        );

        // Raycast down to find the ground
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
        {
            Vector3 finalPos = hit.point + Vector3.up * 0.2f;
            GameObject food = Instantiate(prefab, finalPos, Quaternion.identity);

            currentFood++;
        }
    }
    
    // Call this when food is collected to allow more to spawn
    public void FoodCollected()
    {
        currentFood--;
    }
}
