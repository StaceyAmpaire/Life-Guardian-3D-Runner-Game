using UnityEngine;
using TMPro;

public class ActivityItem : MonoBehaviour
{
    public enum ActivityType { Healthy, Unhealthy }

    [SerializeField] private ActivityType type = ActivityType.Healthy;
    [SerializeField] private AudioClip collectSound; // Renamed from foodSound
    [SerializeField] private string activityName; // Renamed from foodName
    [SerializeField] private float bloodSugarImpact = 10f;

    // nameText will now be assigned automatically if it's a child
    private TextMeshPro nameText; // Using TextMeshPro directly as in user's CollectFood.cs

    private AudioSource itemFX; // Renamed from foodFX

    void Awake()
    {
        // Automatically find the TMP_Text component in children
        nameText = GetComponentInChildren<TextMeshPro>();
        if (nameText == null)
        {
            Debug.LogWarning("No TextMeshPro component found as a child of " + gameObject.name + ". Please ensure your activity prefab has a TextMeshPro child.");
        }

        // Try to get a local AudioSource if one exists on this GameObject
        itemFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Set the text of the TextMeshPro component
        if (nameText != null)
        {
            nameText.text = activityName;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        float impact = 0f;
        // Check if the object hitting the item is the Player
        if (other.GetComponent<PlayerMovement>() != null || other.CompareTag("Player"))
        {
            // Play sound using PlayClipAtPoint for robustness,
            // especially if the item is immediately deactivated.
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            else if (itemFX != null && itemFX.clip != null) // Fallback to local AudioSource if collectSound is not assigned, but itemFX has a clip
            {
                itemFX.Play();
            }
            else if (itemFX == null)
            {
                Debug.LogWarning("ItemFX AudioSource is missing on " + gameObject.name + ". Please assign one in the Inspector or provide an AudioClip.");
            }
            else if (itemFX.clip == null)
            {
                Debug.LogWarning("ItemFX Audio Clip is missing on " + gameObject.name + ". Please assign one in the Inspector.");
            }

            // Update the score and tree impact
            if (type == ActivityType.Healthy)
            {
                MasterInfo.dewCount += 5;
                MasterInfo.healthyCount++;

                MasterInfo.healthyStreak++;
                MasterInfo.unhealthyStreak = 0;

                // 🌳 TREE IMPACT
                MasterInfo.treeLife = Mathf.Clamp(MasterInfo.treeLife + 5, 0, 100);

                PlayerGrowth playerGrowth = FindFirstObjectByType<PlayerGrowth>();
                if (playerGrowth != null)
                {
                    playerGrowth.HandleHealthyStreak(MasterInfo.healthyStreak);
                }
                impact = -bloodSugarImpact; // LOWER sugar
            }
            else
            {
                MasterInfo.dewCount = Mathf.Max(0, MasterInfo.dewCount - 3);
                MasterInfo.unhealthyCount++;

                MasterInfo.unhealthyStreak++;
                MasterInfo.healthyStreak = 0;

                // 🌳 TREE IMPACT
                MasterInfo.treeLife = Mathf.Clamp(MasterInfo.treeLife - 6, 0, 100);

                PlayerGrowth playerGrowth = FindFirstObjectByType<PlayerGrowth>();
                if (playerGrowth != null)
                {
                    playerGrowth.HandleUnhealthyStreak(MasterInfo.unhealthyStreak);
                }
                impact = bloodSugarImpact; // RAISE sugar
            }

            if (MasterInfo.Instance != null)
            {
                MasterInfo.Instance.AdjustBloodSugar(impact); // apply change first
                MasterInfo.Instance.UpdateDewDisplay();       // then update UI
            }

            // Deactivate the item
            gameObject.SetActive(false);
        }
    }
}
