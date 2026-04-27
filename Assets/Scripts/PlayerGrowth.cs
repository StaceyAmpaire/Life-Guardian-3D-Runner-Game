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
        MasterInfo.bodyWeight -= 0.2f;
        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.playerSpeed += speedStep;
            movement.playerSpeed = Mathf.Min(maxSpeed, movement.playerSpeed);
        }
    }

    public void HandleUnhealthyStreak(int streak)
    {
        MasterInfo.bodyWeight += 0.2f;
        MasterInfo.bodyWeight = Mathf.Clamp(MasterInfo.bodyWeight, 1f, 2f);

        ApplyBodySize();

        if (movement != null)
        {
            movement.playerSpeed -= speedStep;
            movement.playerSpeed = Mathf.Max(minSpeed, movement.playerSpeed);
        }
    }

    void ApplyBodySize()
    {
        bodyModel.localScale = new Vector3(
            baseScale.x * MasterInfo.bodyWeight,
            baseScale.y,
            baseScale.z * MasterInfo.bodyWeight
        );
    }
}