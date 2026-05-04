using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Added for cleaner scene code
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

    public static int treeLife = 70; // start healthy-ish

    // This is the variable your MainMenuManager checks
    public static bool tutorialShownThisSession = false;

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
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextObjectInScene();
        FindBloodSugarUIObjectsInScene();
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

    // Finds the Blood Sugar TextMeshPro object and Slider in the current scene.
    public void FindBloodSugarUIObjectsInScene()
    {
        // Find the numerical text display (e.g., "112 mg/dL")
        GameObject valueObj = GameObject.Find("BloodSugarValueText"); // Assumes your numerical Text object is named "BloodSugarValueText"
        if (valueObj != null)
        {
            bloodSugarValueText = valueObj.GetComponent<TMP_Text>();
        }

        // Find the Slider component
        GameObject sliderObj = GameObject.Find("BloodSugarSlider"); // Assumes your Slider GameObject is named "BloodSugarSlider"
        if (sliderObj != null)
        {
            bloodSugarSlider = sliderObj.GetComponent<Slider>();
            if (bloodSugarSlider != null)
            {
                bloodSugarSlider.minValue = minBloodSugar;
                bloodSugarSlider.maxValue = maxBloodSugar;
            }
        }
        UpdateBloodSugarDisplay(); // Update immediately after finding
    }
    // Updates the Blood Sugar UI display (Slider and Text).
    public void UpdateBloodSugarDisplay()
    {
        // Update numerical text display
        if (bloodSugarValueText != null)
        {
            bloodSugarValueText.text = $"{bloodSugar:F0}"; // Display blood sugar value, no decimal for cleaner look

            // Change text color based on blood sugar levels for visual feedback.
            if (bloodSugar < lowBloodSugarThreshold)
            {
                bloodSugarValueText.color = Color.blue; // Low blood sugar
            }
            else if (bloodSugar > normalBloodSugarColorThreshold)
            {
                bloodSugarValueText.color = Color.red; // High blood sugar
            }
            else
            {
                bloodSugarValueText.color = Color.green; // Healthy range
            }
        }

        // Update slider value
        if (bloodSugarSlider != null)
        {
            bloodSugarSlider.value = bloodSugar;
        }
    }

    // Adjusts blood sugar level based on impact (from food) and player's body weight.
    public void AdjustBloodSugar(float impact)
    {
        // The impact of food on blood sugar is inversely proportional to body weight.
        // A higher bodyWeight means the same food impact will have a smaller effect on blood sugar change.
        float adjustedImpact = impact / (1f + (bodyWeight - 1f) * 0.1f); 

        bloodSugar += adjustedImpact;
        // Clamp blood sugar to a realistic and safe range (e.g., 0 to 300 mg/dL).
        bloodSugar = Mathf.Clamp(bloodSugar, 0f, 300f); 

        UpdateBloodSugarDisplay(); // Refresh the UI after adjustment.
    }


    public void UpdateDewDisplay()
    {
        if (dewDisplayText != null)
        {
            dewDisplayText.text = $"Healing Dew: {dewCount}";
        }
    }
}
