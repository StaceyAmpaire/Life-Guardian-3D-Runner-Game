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
    public static int activityFitness = 0;

    // LIFE VALUE - DO NOT REMOVE (Used for tree mechanics)
    public static int treeLife = 100;

    public static bool tutorialShownThisSession = false;

    // ✅ FIX: Unlock tracking variables
    public static bool level2Unlocked = false;
    public static bool level2UnlockAnimationPlayed = false;
    private const string LEVEL2_ANIM_KEY = "Level2UnlockAnimationPlayed"; // Add key variable

    public static string PlayerName { get; private set; } = "";

    private TMP_Text dewDisplayText;
    public static bool USE_SAVE_SYSTEM = true;

    private const string DEW_KEY = "PlayerDew";
private const string TOTAL_DEW_KEY = "PlayerTotalDew";
private const string TREE_LIFE_KEY = "TreeLife";
private const string BODY_WEIGHT_KEY = "BodyWeight";
private const string LEVEL2_KEY = "Level2Unlocked";

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

        if (USE_SAVE_SYSTEM)
{
    LoadData();
}

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
        UpdateDewDisplay();
UpdateLifeDisplay();
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
    if (!level2Unlocked && totalDewCount >= 50)
    {
        level2Unlocked = true;
        SaveData();
        Debug.Log("✅ LEVEL 2 UNLOCKED! Total Dew: " + totalDewCount);

        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.MarkLevel2Unlocked();
        }
    }
}
public static void SetTreeLife(int newLife)
{
    treeLife = Mathf.Clamp(newLife, 0, 100);

    if (Instance != null)
        Instance.UpdateLifeDisplay();

    if (AchievementManager.Instance != null)
        AchievementManager.Instance.NotifyLifeChanged(treeLife);
}

private void LoadData()
{
    dewCount =
        PlayerPrefs.GetInt(DEW_KEY, 0);

    totalDewCount =
        PlayerPrefs.GetInt(TOTAL_DEW_KEY, 0);

    treeLife =
        PlayerPrefs.GetInt(TREE_LIFE_KEY, 100);

    bodyWeight =
        PlayerPrefs.GetFloat(BODY_WEIGHT_KEY, 1f);

    level2Unlocked =
        PlayerPrefs.GetInt(LEVEL2_KEY, 0) == 1;

     level2UnlockAnimationPlayed = PlayerPrefs.GetInt(LEVEL2_ANIM_KEY, 0) == 1;    
}

public static void SaveData()
{
    if (!USE_SAVE_SYSTEM)
        return;

    PlayerPrefs.SetInt(DEW_KEY, dewCount);

    PlayerPrefs.SetInt(
        TOTAL_DEW_KEY,
        totalDewCount);

    PlayerPrefs.SetInt(
        TREE_LIFE_KEY,
        treeLife);

    PlayerPrefs.SetFloat(
        BODY_WEIGHT_KEY,
        bodyWeight);

    PlayerPrefs.SetInt(
        LEVEL2_KEY,
        level2Unlocked ? 1 : 0);

    PlayerPrefs.SetInt(LEVEL2_ANIM_KEY, level2UnlockAnimationPlayed ? 1 : 0);

    PlayerPrefs.Save();
}

[ContextMenu("Delete Save Data")]
private void DeleteSaveData()
{
    PlayerPrefs.DeleteKey(DEW_KEY);
    PlayerPrefs.DeleteKey(TOTAL_DEW_KEY);
    PlayerPrefs.DeleteKey(TREE_LIFE_KEY);
    PlayerPrefs.DeleteKey(BODY_WEIGHT_KEY);
    PlayerPrefs.DeleteKey(LEVEL2_KEY);
    PlayerPrefs.DeleteKey("TutorialSeen");

    // 2. Reset the live runtime variables to 0 immediately
    dewCount = 0;
    totalDewCount = 0;
    level2Unlocked = false;
    treeLife = 100;
    bodyWeight = 1f;

    // 3. Clear all achievement progress and claim records via the UI script
    AchievementsUIManager.ResetAllAchievements();

    PlayerPrefs.Save();

    Debug.Log("Save Data Deleted");
}
}