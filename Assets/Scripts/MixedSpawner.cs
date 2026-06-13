using UnityEngine;

public class MixedSpawner : MonoBehaviour
{
    [Header("Prefabs - Food")]
    public GameObject[] healthyFood;
    public GameObject[] unhealthyFood;

    [Header("Prefabs - Activities")]
    public GameObject[] healthyActivities;
    public GameObject[] unhealthyActivities;

    [Header("Spawn Settings")]
    public int maxItems = 30;
    public float spawnInterval = 0.8f;
    public Transform player;

    [Header("Chances")]
    [Range(0f, 1f)] public float activityChance = 0.8f; // 80% chance to spawn an activity instead of food
    [Range(0f, 1f)] public float unhealthyChance = 0.3f;

    [Header("Lane Settings")]
    public float laneDistance = 4f; 
    public float centerOffset = -6.0f;

    private int currentItems = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
    }

    void SpawnItem()
    {
        if (currentItems >= maxItems) return;

        bool isActivity = Random.value < activityChance;
        bool isUnhealthy = Random.value < unhealthyChance;

        GameObject prefab;

        if (isActivity)
        {
            prefab = isUnhealthy 
                ? unhealthyActivities[Random.Range(0, unhealthyActivities.Length)] 
                : healthyActivities[Random.Range(0, healthyActivities.Length)];
        }
        else
        {
            prefab = isUnhealthy 
                ? unhealthyFood[Random.Range(0, unhealthyFood.Length)] 
                : healthyFood[Random.Range(0, healthyFood.Length)];
        }

        int randomLane = Random.Range(0, 3);
        float spawnX = (randomLane - 1) * laneDistance + centerOffset;

        Vector3 spawnPos = new Vector3(
            spawnX,
            30f,
            player.position.z + Random.Range(35f, 65f)
        );

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
        {
            Vector3 finalPos = hit.point + Vector3.up * 0.2f;
            Instantiate(prefab, finalPos, Quaternion.identity);
            currentItems++;
        }
    }

    public void ItemCollected()
    {
        currentItems--;
    }
}