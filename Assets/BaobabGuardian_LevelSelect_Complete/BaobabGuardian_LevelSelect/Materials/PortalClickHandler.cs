using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalClickHandler : MonoBehaviour
{
    [SerializeField] private string pathName; // "Prevention" or "Management"
    [SerializeField] private string sceneToLoad = "LevelSelectScene"; // Scene name to load

    private void OnMouseDown()
    {
        Debug.Log($"Clicked {pathName} portal! Loading {sceneToLoad}...");
        LoadLevelSelectScene();
    }

    private void LoadLevelSelectScene()
    {
        // Load the LevelSelectScene
        SceneManager.LoadScene(sceneToLoad);
    }
}
