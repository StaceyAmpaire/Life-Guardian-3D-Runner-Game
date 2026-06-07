using UnityEngine;

public class FoodVisualDisplay : MonoBehaviour
{
    void Start()
    {
        // Make sprite stand upright
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void Update()
    {
        // Keep facing camera
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0f, 180f, 0f);
        }
    }
}