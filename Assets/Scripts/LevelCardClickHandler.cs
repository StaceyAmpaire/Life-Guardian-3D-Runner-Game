using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelCardClickHandler : MonoBehaviour
{
    [SerializeField] private string pathName; // "Prevention" or "Management" (for reference)
    [SerializeField] private string sceneToLoad = "Run"; // Scene to load
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnLevelCardClicked);
    }

    private void OnLevelCardClicked()
    {
        Debug.Log($"Clicked level card from {pathName} path! Loading {sceneToLoad}...");
        
        // Save scene name for LoadingManager
        PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
        
        // Load the LOADING scene (not the game scene directly)
        SceneManager.LoadScene("LoadingScene");  // ← Changed this!
    }
}
