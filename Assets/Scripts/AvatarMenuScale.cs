using UnityEngine;

public class AvatarMenuScale : MonoBehaviour
{
    private Vector3 originalScale;
    private bool scaleInitialized = false;

    void Start()
    {
        // Store the ORIGINAL scale once
        if (!scaleInitialized)
        {
            originalScale = transform.localScale;
            scaleInitialized = true;
        }
        
        UpdateAvatarScale();
    }

    void Update()
    {
        UpdateAvatarScale();
    }

    void UpdateAvatarScale()
    {
        // Always multiply from the ORIGINAL scale, not the current scale
        transform.localScale = new Vector3(
            originalScale.x * MasterInfo.bodyWeight,
            originalScale.y,
            originalScale.z * MasterInfo.bodyWeight
        );
    }
}
