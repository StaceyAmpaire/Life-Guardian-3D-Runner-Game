using UnityEngine;
using TMPro;

public class CollectFood : MonoBehaviour
{
    public enum FoodType { Healthy, Unhealthy }

    [SerializeField] private FoodType type = FoodType.Healthy;
    [SerializeField] private AudioClip foodSound;
    [SerializeField] private string foodName;

    private TextMeshPro nameText;
    private AudioSource foodFX;

    void Awake()
    {
        nameText = GetComponentInChildren<TextMeshPro>();

        if (nameText == null)
        {
            Debug.LogWarning("No TMP_Text component found as a child of " + gameObject.name);
        }

        foodFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (nameText != null)
        {
            nameText.text = foodName;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null || other.CompareTag("Player"))
        {
            // Play sound
            if (foodSound != null)
            {
                AudioSource.PlayClipAtPoint(foodSound, transform.position);
            }
            else if (foodFX != null && foodFX.clip != null)
            {
                foodFX.Play();
            }

            if (type == FoodType.Healthy)
            {
                MasterInfo.dewCount += 5;
                MasterInfo.healthyCount++;

                MasterInfo.healthyStreak++;
                MasterInfo.unhealthyStreak = 0;

                // Increase Life
                MasterInfo.treeLife = Mathf.Clamp(MasterInfo.treeLife + 5, 0, 100);

                // Trigger fast UI update
                if (MasterInfo.Instance != null)
                {
                    MasterInfo.Instance.UpdateLifeDisplay();
                }

                PlayerGrowth playerGrowth = FindFirstObjectByType<PlayerGrowth>();
                if (playerGrowth != null)
                {
                    playerGrowth.HandleHealthyStreak(MasterInfo.healthyStreak);
                }
            }
            else
            {
                MasterInfo.dewCount = Mathf.Max(0, MasterInfo.dewCount - 3);
                MasterInfo.unhealthyCount++;

                MasterInfo.unhealthyStreak++;
                MasterInfo.healthyStreak = 0;

                // Decrease Life
                MasterInfo.treeLife = Mathf.Clamp(MasterInfo.treeLife - 6, 0, 100);

                // Trigger fast UI update
                if (MasterInfo.Instance != null)
                {
                    MasterInfo.Instance.UpdateLifeDisplay();
                }

                PlayerGrowth playerGrowth = FindFirstObjectByType<PlayerGrowth>();
                if (playerGrowth != null)
                {
                    playerGrowth.HandleUnhealthyStreak(MasterInfo.unhealthyStreak);
                }
            }

            if (MasterInfo.Instance != null)
            {
                MasterInfo.Instance.UpdateDewDisplay();
            }

            gameObject.SetActive(false);
        }
    }
}
