using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for the New Input System

public class ClickScript : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "EnvironmentChoice"; // Scene name to load

    void Update()
    {
        Vector2 inputScreenPosition = Vector2.zero;
        bool inputDetected = false;

        // 1. Check for PC Mouse Click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            inputScreenPosition = Mouse.current.position.ReadValue();
            inputDetected = true;
        }
        // 2. Check for Mobile Finger Touch (Explicitly using the New Input System name)
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            inputScreenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            inputDetected = true;
        }

        // 3. Process the Raycast if an input happened
        if (inputDetected)
        {
            // Create a ray from the camera through the input position
            Ray mouseRay = Camera.main.ScreenPointToRay(inputScreenPosition);

            // Perform the 2D raycast
            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            // Check if the ray hit a 2D collider
            if (hit.collider != null)
            {
                Transform clickedObject = hit.collider.transform;
                Debug.Log($"Pressed {clickedObject.name}! Loading {sceneToLoad}...");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
