using UnityEngine;
using TMPro;

public class CollectFood : MonoBehaviour
{
    public enum FoodType { Healthy, Unhealthy }

    [SerializeField] private FoodType type = FoodType.Healthy;

// NEW
[SerializeField] private int dewValue = 5;
[SerializeField] private int lifeValue = 5;
    [SerializeField] private AudioClip foodSound;
    [SerializeField] private string foodName;
    [SerializeField]
[TextArea]
private string nutritionMessage;

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
       bool canPlaySfx = AudioSettingsManager.Instance == null ||
                  AudioSettingsManager.Instance.SfxEnabled;

if (canPlaySfx)
{
    if (foodSound != null)
    {
        AudioSource.PlayClipAtPoint(foodSound, transform.position);
    }
    else if (foodFX != null && foodFX.clip != null)
    {
        foodFX.Play();
    }
}

        // Apply custom food values
        MasterInfo.dewCount =
    Mathf.Max(0, MasterInfo.dewCount + dewValue);

MasterInfo.totalDewCount =
    Mathf.Max(0, MasterInfo.totalDewCount + dewValue);

    MasterInfo.CheckAndUnlockLevel2();

        MasterInfo.SetTreeLife(MasterInfo.treeLife + lifeValue);
        MasterInfo.SaveData();

        // Healthy food
        if (lifeValue > 0)
        {
            MasterInfo.healthyCount++;

            MasterInfo.healthyStreak++;
            MasterInfo.unhealthyStreak = 0;

            PlayerGrowth playerGrowth = FindFirstObjectByType<PlayerGrowth>();

            if (playerGrowth != null)
            {
                playerGrowth.HandleHealthyStreak(MasterInfo.healthyStreak);
            }
        }
        // Unhealthy food
else
{
    MasterInfo.unhealthyCount++;

    MasterInfo.unhealthyStreak++;
    MasterInfo.healthyStreak = 0;

    // ⚠ Show warning only for very unhealthy foods
    if (lifeValue <= -7)
    {
        if (NutritionWarningUI.Instance != null)
        {
            NutritionWarningUI.Instance.ShowWarning(nutritionMessage);
        }
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
        if (AchievementManager.Instance != null)
{
    bool isHealthy = lifeValue > 0;
    int dewEarned = dewValue > 0 ? dewValue : 0;

    AchievementManager.Instance.RegisterFoodChoice(
        foodName,
        isHealthy,
        dewEarned
    );
}

        gameObject.SetActive(false);
    }
}
}
