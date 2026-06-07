using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food Prefabs")]
    public GameObject[] foodPrefabs;

    [Header("Player")]
    public Transform player;

    [Header("Road Settings")]
    public float roadLength = 3000f;

    // Where food spawning starts
    public float startZ = 50f;

    // Smaller value = foods closer together
    public float distanceBetweenFoods = 0.5f;

    [Header("Food Position")]
    public float spawnY = 2.5f;

    // LEFT and RIGHT lanes
    public float[] laneXPositions = { 405f, 409f };

    private bool hasSpawned = false;

    public void StartFoodSpawning()
    {
        if (hasSpawned) return;

        hasSpawned = true;

        SpawnFoodsOnRoad();
    }

    void SpawnFoodsOnRoad()
    {
        if (foodPrefabs.Length == 0)
        {
            Debug.LogError("No food prefabs assigned to FoodSpawner.");
            return;
        }

        // CALCULATE TOTAL FOODS
        int totalFoods =
            Mathf.FloorToInt(roadLength / distanceBetweenFoods);

        for (int i = 0; i < totalFoods; i++)
        {
            // RANDOM FOOD
            int foodIndex =
                Random.Range(0, foodPrefabs.Length);

            // RANDOM LANE
            int laneIndex =
                Random.Range(0, laneXPositions.Length);

            // POSITION ALONG ROAD
            float zPosition =
                startZ + (i * distanceBetweenFoods);

            Vector3 spawnPosition =
                new Vector3(
                    laneXPositions[laneIndex],
                    spawnY,
                    zPosition
                );

            Instantiate(
                foodPrefabs[foodIndex],
                spawnPosition,
                Quaternion.identity
            );
        }

        Debug.Log(totalFoods + " foods spawned on the road.");
    }
}