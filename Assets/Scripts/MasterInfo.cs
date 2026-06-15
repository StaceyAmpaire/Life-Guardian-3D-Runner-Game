using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterInfo : MonoBehaviour
{
    public static int dewCount = 0;
    public static int totalDewCount = 0;

    public static int healthyCount = 0;
    public static int unhealthyCount = 0;

    public static int healthyActivityCount = 0;
    public static int unhealthyActivityCount = 0;

    public static int healthyStreak = 0;
    public static int unhealthyStreak = 0;

    public static float bodyWeight = 1f;

    // LIFE VALUE - DO NOT REMOVE (Used for tree mechanics)
    public static int treeLife = 100;

    public static bool tutorialShownThisSession = false;

    // ✅ FIX: Unlock tracking variables
    public static bool level2Unlocked = false;
    public static bool level2UnlockAnimationPlayed = false;

    public static string PlayerName { get; private set; } = "";

    private TMP_Text dewDisplayText;

    // These references are now handled by PlayerLifeManager for faster updates
    // private TMP_Text lifeValueText;
    // private Slider lifeSlider;

    public static MasterInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (UserProfileManager.Instance != null)
        {
            PlayerName = UserProfileManager.Instance.PlayerName;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextObjectInScene();
        
        // Fast UI sync on scene load
        if (PlayerLifeManager.Instance != null)
        {
            PlayerLifeManager.Instance.UpdateLifeUI(treeLife);
        }

        if (UserProfileManager.Instance != null)
        {
            UserProfileManager.Instance.UpdatePlayerNameDisplay();
        }
    }

    public void FindTextObjectInScene()
    {
        GameObject obj = GameObject.Find("HealingDewText");

        if (obj != null)
        {
            dewDisplayText = obj.GetComponent<TMP_Text>();
            UpdateDewDisplay();
        }
    }

    // This method is now simplified as PlayerLifeManager handles the UI
    public void UpdateLifeDisplay()
    {
        if (PlayerLifeManager.Instance != null)
        {
            PlayerLifeManager.Instance.UpdateLifeUI(treeLife);
        }
    }

    public void UpdateDewDisplay()
    {
        if (dewDisplayText != null)
        {
            dewDisplayText.text = $"Healing Dew: {dewCount}";
        }
    }

    public static void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public static void ResetRunData()
    {
        dewCount = 0;

        healthyCount = 0;
        unhealthyCount = 0;

        healthyActivityCount = 0;
        unhealthyActivityCount = 0;

        healthyStreak = 0;
        unhealthyStreak = 0;

        treeLife = 100;
    }

    // ✅ FIX: Method to check and set unlock when dew reaches 50
    public static void CheckAndUnlockLevel2()
    {
        // Only unlock if not already unlocked
        if (!level2Unlocked && totalDewCount >= 50)
        {
            level2Unlocked = true;
            Debug.Log("✅ LEVEL 2 UNLOCKED! Total Dew: " + totalDewCount);
        }
    }
}