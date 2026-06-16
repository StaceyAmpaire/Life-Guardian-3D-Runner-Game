using UnityEngine;
using TMPro;

public class CollectActivity : MonoBehaviour
{
    [SerializeField] private string activityName;
    [SerializeField]
[TextArea]
private string activityMessage;

    [Header("Rewards / Penalties")]
    [SerializeField] private int dewValue = 10;
    [SerializeField] private int lifeValue = 8;

    [Header("Audio")]
    [SerializeField] private AudioClip activitySound;

    private TextMeshPro nameText;
    private AudioSource audioFX;

    void Awake()
    {
        nameText = GetComponentInChildren<TextMeshPro>();
        audioFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (nameText != null)
        {
            nameText.text = activityName;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.GetComponent<PlayerMovement>() != null)
        {
            // Play sound
            if (activitySound != null)
            {
                AudioSource.PlayClipAtPoint(
                    activitySound,
                    transform.position);
            }
            else if (audioFX != null &&
                     audioFX.clip != null)
            {
                audioFX.Play();
            }

            // Apply activity values
            MasterInfo.dewCount =
    Mathf.Max(0,
    MasterInfo.dewCount + dewValue);

MasterInfo.totalDewCount =
    Mathf.Max(0,
    MasterInfo.totalDewCount + dewValue);
            MasterInfo.treeLife =
                Mathf.Clamp(
                    MasterInfo.treeLife + lifeValue,
                    0,
                    100);

            // Count healthy vs unhealthy activities
            if (lifeValue > 0)
{
    MasterInfo.healthyCount++;
}
else
{
    MasterInfo.unhealthyCount++;

    if (NutritionWarningUI.Instance != null)
    {
        NutritionWarningUI.Instance.ShowWarning(activityMessage);
    }
}

            // Update UI
            if (MasterInfo.Instance != null)
            {
                MasterInfo.Instance.UpdateLifeDisplay();
                MasterInfo.Instance.UpdateDewDisplay();
            }

            // Notify ActivitySpawner
            ActivitySpawner spawner =
                FindFirstObjectByType<ActivitySpawner>();

            if (spawner != null)
            {
                spawner.ActivityCollected();
            }

            gameObject.SetActive(false);
        }
    }
}