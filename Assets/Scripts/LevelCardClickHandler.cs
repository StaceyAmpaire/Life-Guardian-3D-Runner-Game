using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelCardClickHandler : MonoBehaviour
{
    [SerializeField] private string pathName; // "Prevention" or "Management" (for reference)
    [SerializeField] private string sceneToLoad = "Run"; // Scene to load
    [SerializeField] private bool isActivityLevel = false; // Set to true for activity level cards
    
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnLevelCardClicked);
    }

    private void OnLevelCardClicked()
{
    Debug.Log("Clicked: " + gameObject.name);

    if (isActivityLevel && !MasterInfo.level2Unlocked)
    {
        Debug.Log("LOCKED");
        return;
    }

    MasterInfo.ResetRunData();

    PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
    SceneManager.LoadScene("LoadingScene");
}
}
