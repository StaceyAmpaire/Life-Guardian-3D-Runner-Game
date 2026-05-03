using UnityEngine;
using TMPro;

public class CollectFood : MonoBehaviour
{
    public enum FoodType { Healthy, Unhealthy }

    [SerializeField] private FoodType type = FoodType.Healthy;
    [SerializeField] private AudioClip foodSound; // Assign the AudioClip here for PlayClipAtPoint
    [SerializeField] private string foodName; // Assign the name in the Inspector
    [SerializeField] private float bloodSugarImpact = 10f;
   

    // nameText will now be assigned automatically
    private TextMeshPro nameText;

    private AudioSource foodFX; // Used if foodSound is not assigned, or for other local sounds

    void Awake()
    {
        // Automatically find the TMP_Text component in children
        nameText = GetComponentInChildren<TextMeshPro>();
        if (nameText == null)
        {
            Debug.LogWarning("No TMP_Text component found as a child of " + gameObject.name + ". Please ensure your food prefab has a TextMeshPro child.");
        }

        // Try to get a local AudioSource if one exists on this GameObject
        foodFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Set the text of the TextMeshPro component
        if (nameText != null)
        {
            nameText.text = foodName;
        }
    }

    void OnTriggerEnter(Collider other)
    {
         float impact = 0f;
        // Check if the object hitting the food is the Player
        if (other.GetComponent<PlayerMovement>() != null || other.CompareTag("Player"))
        {
            // Play sound using PlayClipAtPoint for robustness, 
            // especially if the food item is immediately deactivated.
            if (foodSound != null)
            {
                AudioSource.PlayClipAtPoint(foodSound, transform.position);
            }
            else if (foodFX != null && foodFX.clip != null) // Fallback to local AudioSource if foodSound is not assigned, but foodFX has a clip
            {
                foodFX.Play();
            }
            else if (foodFX == null)
            {
                Debug.LogWarning("FoodFX AudioSource is missing on " + gameObject.name + ". Please assign one in the Inspector or provide an AudioClip.");
            }
            else if (foodFX.clip == null)
            {
                Debug.LogWarning("FoodFX Audio Clip is missing on " + gameObject.name + ". Please assign one in the Inspector.");
            }

            // Update the score and tree impact
            if (type == FoodType.Healthy)
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
                impact = -bloodSugarImpact; //  LOWER sugar
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

            // Deactivate the food item
            gameObject.SetActive(false);
        }
    }
}
