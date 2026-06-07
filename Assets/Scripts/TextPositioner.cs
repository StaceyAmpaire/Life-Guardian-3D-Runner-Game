using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class TextPositioner : MonoBehaviour
{
    [Header("References")]
    public Renderer mainRenderer;
    public GameObject textObject;

    [Header("Position Fine-Tuning")]
    public float verticalOffset = 1.0f;
    public float horizontalOffset = 0.0f;
    public float depthOffset = 0.0f;

    [Header("Size Control")]
    [Tooltip("Adjust this to make text smaller or larger.")]
    public float textScale = 0.1f;

    void Update()
    {
        if (mainRenderer != null && textObject != null)
        {
            // Position Logic
            float topY = mainRenderer.bounds.max.y;
            Vector3 center = mainRenderer.bounds.center;
            textObject.transform.position = new Vector3(
                center.x + horizontalOffset, 
                topY + verticalOffset, 
                center.z + depthOffset
            );

            // Scale Logic
            textObject.transform.localScale = new Vector3(textScale, textScale, textScale);
        }
    }
}
