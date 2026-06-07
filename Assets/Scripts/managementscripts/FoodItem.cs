using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public string foodName = "Apple";

    [Header("Food Score")]
    public int scorePoints = 100;

    [Header("Food Type")]
    public bool healthyFood = true;

    [TextArea(2, 4)]
    public string foodMessage =
        "Apples provide fibre and support steady energy.";

    private bool collected = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // ADD SCORE WRITTEN ON FOOD
            TopBarManager topBar =
                FindObjectOfType<TopBarManager>();

            if (topBar != null)
            {
                topBar.AddScore(scorePoints);
            }

            // PLAYER HEALTH EFFECT
            PlayerRoadMovement player =
                other.GetComponent<PlayerRoadMovement>();

            if (player != null)
            {
                if (healthyFood)
                {
                    player.TakeHealthyChoice(scorePoints * 0.05f);

                    AudioClip healthyClip =
                        Resources.Load<AudioClip>(
                            "Audio/FX/food-humm-129"
                        );

                    if (healthyClip != null)
                        audioSource.PlayOneShot(healthyClip);
                }
                else
                {
                    player.TakeWrongChoice(scorePoints * 0.05f);

                    AudioClip unhealthyClip =
                        Resources.Load<AudioClip>(
                            "Audio/FX/hungry-man-eating-2252"
                        );

                    if (unhealthyClip != null)
                        audioSource.PlayOneShot(unhealthyClip);
                }
            }

            // SAVE FOOD INFO
            FoodCollectionManager manager =
                FindObjectOfType<FoodCollectionManager>();

            if (manager != null)
            {
                manager.RecordFood(
                    foodName,
                    foodMessage,
                    scorePoints,
                    healthyFood
                );
            }

            Destroy(gameObject);
        }
    }
}