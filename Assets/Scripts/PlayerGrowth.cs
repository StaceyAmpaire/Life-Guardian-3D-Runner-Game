using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    public PlayerMovement movement;
    public Transform bodyModel;

    public float speedStep = 1f;
    public float maxSpeed = 6f;
    public float minSpeed = 2f;

    Vector3 baseScale = new Vector3(1f, 1f, 1f);

    void Start()
    {
        if (bodyModel == null)
            bodyModel = transform;

        ApplyBodySize();
    }

    public void HandleHealthyStreak(int streak)
    {
        // gentle decrease
        MasterInfo.bodyWeight -= 0.2f;

        // ✅ MIN = 1 (original size)
        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2.5f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.playerSpeed += speedStep;
            movement.playerSpeed = Mathf.Min(maxSpeed, movement.playerSpeed);
        }
    }

    public void HandleUnhealthyStreak(int streak)
    {
        // strong increase
        MasterInfo.bodyWeight += 0.4f;

        // ✅ MIN = 1 (original size)
        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2.5f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.playerSpeed -= speedStep;
            movement.playerSpeed = Mathf.Max(minSpeed, movement.playerSpeed);
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