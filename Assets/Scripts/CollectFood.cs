using UnityEngine;

public class CollectFood : MonoBehaviour
{
    public enum FoodType { Healthy, Unhealthy }

    [SerializeField] private FoodType type = FoodType.Healthy;
    [SerializeField] private AudioSource foodFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null || other.CompareTag("Player"))
        {
            if (foodFX != null)
            {
                foodFX.Play();
            }

            if (type == FoodType.Healthy)
{
    MasterInfo.dewCount += 5;
    MasterInfo.healthyCount++;

    MasterInfo.healthyStreak++;
    MasterInfo.unhealthyStreak = 0;

    FindFirstObjectByType<PlayerGrowth>().HandleHealthyStreak(MasterInfo.healthyStreak);
}
else
{
    MasterInfo.dewCount = Mathf.Max(0, MasterInfo.dewCount - 3);
    MasterInfo.unhealthyCount++;

    MasterInfo.unhealthyStreak++;
    MasterInfo.healthyStreak = 0;

    FindFirstObjectByType<PlayerGrowth>().HandleUnhealthyStreak(MasterInfo.unhealthyStreak);
}
            

            if (MasterInfo.Instance != null)
            {
                MasterInfo.Instance.UpdateDewDisplay();
            }

            gameObject.SetActive(false);
        }
    }
}