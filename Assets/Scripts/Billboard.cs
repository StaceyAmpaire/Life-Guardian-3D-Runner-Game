using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Find the main camera's transform. Assumes your main camera is tagged 'MainCamera'.
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Main Camera not found or not tagged 'MainCamera'. BillboardText will not function.");
            enabled = false; // Disable the script if no camera is found
        }
    }

    void LateUpdate()
    {
        if (mainCameraTransform != null)
        {
            // Make the text face the camera
            transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);

            // Optional: If you want the text to always be upright (not tilt with camera pitch)
            // transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
