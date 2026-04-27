using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Added for cleaner scene code

public class MasterInfo : MonoBehaviour
{
    // These will persist as long as the game is running
    public static int dewCount = 0;
    public static int healthyCount = 0;
    public static int unhealthyCount = 0;
    public static int healthyStreak = 0;
    public static int unhealthyStreak = 0;
    public static float bodyWeight = 1f;

    // This is the variable your MainMenuManager checks
    public static bool tutorialShownThisSession = false;

    private TMP_Text dewDisplayText;
    public static MasterInfo Instance { get; private set; }

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextObjectInScene();
    }

    public void FindTextObjectInScene()
    {
        // Note: Find only works on ACTIVE objects. 
        // If your text is hidden, this will return null.
        GameObject obj = GameObject.Find("HealingDewText");
        if (obj != null)
        {
            dewDisplayText = obj.GetComponent<TMP_Text>();
            UpdateDewDisplay();
        }
    }

    public void UpdateDewDisplay()
    {
        if (dewDisplayText != null)
        {
            dewDisplayText.text = $"Healing Dew: {dewCount}";
        }
    }
}
