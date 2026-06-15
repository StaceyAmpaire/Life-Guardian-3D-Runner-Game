using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    public PlayerMovement movement;
    public Transform bodyModel;

    public float speedStep = 1f;
    public float maxSpeed = 15f;
    public float minSpeed = 5f;

    // Will store the avatar's ORIGINAL size
    private Vector3 baseScale;

    void Start()
{
    // Wait one frame for AvatarLoader to finish
    Invoke("FindActiveAvatar", 0.01f);
}

void FindActiveAvatar()
{
    foreach (Transform child in transform)
    {
        if ((child.gameObject.name == "Running (1)" || child.gameObject.name == "Running (2)") 
            && child.gameObject.activeInHierarchy)
        {
            bodyModel = child;
            Debug.Log("✅ Found active avatar: " + bodyModel.name);
            break;
        }
    }

    if (bodyModel == null)
        bodyModel = transform;

    baseScale = bodyModel.localScale;
    ApplyBodySize();
}



    public void HandleHealthyStreak(int streak)
    {
        // gentle decrease
        MasterInfo.bodyWeight -= 0.2f;

        // minimum = normal size
        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2.5f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.forwardSpeed += speedStep;
            movement.forwardSpeed = Mathf.Min(maxSpeed, movement.forwardSpeed);
        }
    }

    public void HandleUnhealthyStreak(int streak)
    {
        // stronger increase
        MasterInfo.bodyWeight += 0.4f;

        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2.5f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.forwardSpeed -= speedStep;
            movement.forwardSpeed = Mathf.Max(minSpeed, movement.forwardSpeed);
        }
    }

    void ApplyBodySize()
    {
        float weight = MasterInfo.bodyWeight;

        bodyModel.localScale = new Vector3(
            baseScale.x * weight,
            baseScale.y * Mathf.Lerp(1f, 1.2f, (weight - 1f) / (2.5f - 1f)),
            baseScale.z * weight
        );
    }
}