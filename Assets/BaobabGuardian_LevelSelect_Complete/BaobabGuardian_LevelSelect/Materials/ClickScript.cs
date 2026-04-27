using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for the new Input System

public class ClickScript : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "LevelSelectScene"; // Scene name to load

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was pressed this frame using the new Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get the current mouse position from the new Input System
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera through the mouse position
            Ray mouseRay = Camera.main.ScreenPointToRay(mouseScreenPosition);

            // Perform the 2D raycast
            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            // Check if the ray hit a 2D collider
            if (hit.collider != null)
            {
                Transform clickedObject = hit.collider.transform;
                Debug.Log($"Clicked {clickedObject.name}! Loading {sceneToLoad}...");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
