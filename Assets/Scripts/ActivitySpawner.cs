using UnityEngine;

public class ActivitySpawner : MonoBehaviour
{
    [Header("Activity Prefabs")]
    public GameObject[] healthyActivities;
    public GameObject[] unhealthyActivities;

    [Header("Spawn Settings")]
    public int maxActivities = 25;
    public float spawnInterval = 1f;
    public Transform player;

    [Header("Spawn Chances")]
    [Range(0f, 1f)]
    public float unhealthyChance = 0.3f;

    [Header("Lane Settings")]
    public float laneDistance = 4f;
    public float centerOffset = -6f;

    private int currentActivities = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnActivity), 1f, spawnInterval);
    }

    void SpawnActivity()
    {
        if (currentActivities >= maxActivities)
            return;

        bool spawnUnhealthy =
            Random.value < unhealthyChance;

        GameObject prefab =
            spawnUnhealthy
            ? unhealthyActivities[Random.Range(0, unhealthyActivities.Length)]
            : healthyActivities[Random.Range(0, healthyActivities.Length)];

        int randomLane = Random.Range(0, 3);

        float spawnX =
            (randomLane - 1) * laneDistance + centerOffset;

        Vector3 spawnPos = new Vector3(
            spawnX,
            30f,
            player.position.z + Random.Range(30f, 60f)
        );

        if (Physics.Raycast(
            spawnPos,
            Vector3.down,
            out RaycastHit hit,
            50f))
        {
            Vector3 finalPos =
                hit.point + Vector3.up * 1f;

            Instantiate(
                prefab,
                finalPos,
                prefab.transform.rotation);

            currentActivities++;
        }
    }

    public void ActivityCollected()
    {
        currentActivities--;
    }
}