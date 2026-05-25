using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterInfo : MonoBehaviour
{
    // These will persist as long as the game is running
    public static int dewCount = 0;
    public static int healthyCount = 0;
    public static int unhealthyCount = 0;
    public static int healthyStreak = 0;
    public static int unhealthyStreak = 0;
    public static float bodyWeight = 1f;

    public static int treeLife = 100; // start healthy-ish

    // This is the variable your MainMenuManager checks
    public static bool tutorialShownThisSession = false;

    // Player Profile
    public static string PlayerName { get; private set; } = ""; // New: Stores player's name

    private TMP_Text dewDisplayText;

    // Blood Sugar variables
    public static float bloodSugar = 100f; // Initial blood sugar level (e.g., mg/dL)
    public static float minBloodSugar = 70f; // Minimum value for the slider
    public static float maxBloodSugar = 180f; // Maximum value for the slider (e.g., post-meal high)
    public static float normalBloodSugarColorThreshold = 120f; // Upper limit for 'normal' green color
    public static float lowBloodSugarThreshold = 80f; // Lower limit for 'normal' green color

    private TMP_Text bloodSugarValueText; // Reference for the numerical Blood Sugar UI Text (e.g., 112 mg/dL)
    private Slider bloodSugarSlider; // Reference for the Blood Sugar UI Slider
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

        // Initialize PlayerName from UserProfileManager if it exists
        if (UserProfileManager.Instance != null)
        {
            PlayerName = UserProfileManager.Instance.PlayerName;
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextObjectInScene();
        FindBloodSugarUIObjectsInScene();
        
        // New: Update player name display if it exists in the scene
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

    public void FindBloodSugarUIObjectsInScene()
    {
        GameObject valueObj = GameObject.Find("BloodSugarValueText");
        if (valueObj != null)
        {
            bloodSugarValueText = valueObj.GetComponent<TMP_Text>();
        }

        GameObject sliderObj = GameObject.Find("BloodSugarSlider");
        if (sliderObj != null)
        {
            bloodSugarSlider = sliderObj.GetComponent<Slider>();
            if (bloodSugarSlider != null)
            {
                bloodSugarSlider.minValue = minBloodSugar;
                bloodSugarSlider.maxValue = maxBloodSugar;
            }
        }
        UpdateBloodSugarDisplay();
    }

    public void UpdateBloodSugarDisplay()
    {
        if (bloodSugarValueText != null)
        {
            bloodSugarValueText.text = $"{bloodSugar:F0}";

            if (bloodSugar < lowBloodSugarThreshold)
            {
                bloodSugarValueText.color = Color.blue;
            }
            else if (bloodSugar > normalBloodSugarColorThreshold)
            {
                bloodSugarValueText.color = Color.red;
            }
            else
            {
                bloodSugarValueText.color = Color.green;
            }
        }

        if (bloodSugarSlider != null)
        {
            bloodSugarSlider.value = bloodSugar;
        }
    }

    public void AdjustBloodSugar(float impact)
    {
        float adjustedImpact = impact / (1f + (bodyWeight - 1f) * 0.1f);

        bloodSugar += adjustedImpact;
        bloodSugar = Mathf.Clamp(bloodSugar, 0f, 300f);

        UpdateBloodSugarDisplay();
    }

    public void UpdateDewDisplay()
    {
        if (dewDisplayText != null)
        {
            dewDisplayText.text = $"Healing Dew: {dewCount}";
        }
    }

    // New: Method to update player name in MasterInfo
    public static void SetPlayerName(string name)
    {
        PlayerName = name;
    }
}
