using UnityEngine;
using TMPro;
using System.Collections;

public class PopupUI : MonoBehaviour
{
    [Header("Popup")]
    public GameObject popupPanel;

    [Header("Player Name")]
    public TMP_InputField nameInputField;
    public TMP_Text playerNameText;

    [Header("Feedback")]
    public GameObject feedbackPanel;
    public TMP_Text feedbackText;

    [Header("Reward Cooldown")]
    public float rewardCooldownMinutes = 5f;

    [Header("Audio UI")]
public TMP_Text musicButtonText;
public TMP_Text sfxButtonText;

    private string lastRewardKey = "LastFoodRewardTime";

    private void Start()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

#if UNITY_EDITOR
        // Hold Shift when pressing Play if you want to reset editor test data
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerPrefs.DeleteKey("PlayerName");
            PlayerPrefs.DeleteKey(lastRewardKey);
        }
#endif

        
        LoadPlayerName();
        UpdateAudioButtonTexts();
    }

    public void OpenPopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(true);

        if (nameInputField != null)
            nameInputField.ActivateInputField();

            UpdateAudioButtonTexts();
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    // =========================================================
    // PLAYER NAME
    // =========================================================
    public void SavePlayerName()
    {
        string enteredName = nameInputField.text;

        if (!string.IsNullOrWhiteSpace(enteredName))
        {
            playerNameText.text = enteredName;
            MasterInfo.SetPlayerName(enteredName);

            PlayerPrefs.SetString("PlayerName", enteredName);
            PlayerPrefs.Save();
        }
    }

    void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");

            playerNameText.text = savedName;
            MasterInfo.SetPlayerName(savedName);

            if (nameInputField != null)
                nameInputField.text = savedName;
        }
        else
        {
            playerNameText.text = "Set Name";
        }
    }

    // =========================================================
    // FOOD CHOICE
    // =========================================================
    public void ChooseApple()      => ChooseFood("Apple", true, 10);
    public void ChooseAvocado()    => ChooseFood("Avocado", true, 9);
    public void ChooseWatermelon() => ChooseFood("Watermelon", true, 10);
    public void ChooseFish()       => ChooseFood("Fish", true, 10);

    public void ChooseBurger()     => ChooseFood("Burger", false, 3);
    public void ChooseFries()      => ChooseFood("Fries", false, 3);
    public void ChooseSoda()       => ChooseFood("Soda", false, 1);
    public void ChooseChocolate()  => ChooseFood("Chocolate", false, 4);

    private void ChooseFood(string foodName, bool isHealthy, int dewReward)
    {
        if (!CanClaimReward())
        {
            StartCoroutine(
                ShowFeedback("You already made a food choice recently. Please try again later.")
            );
            return;
        }

        // Give dew reward
        MasterInfo.dewCount += dewReward;
        MasterInfo.totalDewCount += dewReward;

        // Optional tracking of healthy/unhealthy daily choices
        if (isHealthy)
            MasterInfo.healthyCount++;
        else
            MasterInfo.unhealthyCount++;

        // Update achievements if you want daily popup food choices
        // to count toward food achievements too
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.RegisterFoodChoice(
                foodName,
                isHealthy,
                dewReward
            );
        }

        // Check dew-based unlocks
        MasterInfo.CheckAndUnlockLevel2();

        // Refresh dew text in UI
        if (MasterInfo.Instance != null)
            MasterInfo.Instance.UpdateDewDisplay();

        SaveRewardTime();

        StartCoroutine(
            ShowFeedback($"You chose {foodName}! +{dewReward} Healing Dew")
        );

        Debug.Log($"Food chosen: {foodName} | Healthy: {isHealthy} | Reward: {dewReward}");
    }

    // =========================================================
    // COOLDOWN
    // =========================================================
    bool CanClaimReward()
    {
        if (!PlayerPrefs.HasKey(lastRewardKey))
            return true;

        string savedTime = PlayerPrefs.GetString(lastRewardKey);
        System.DateTime lastTime = System.DateTime.Parse(savedTime);

        double minutesPassed =
            (System.DateTime.Now - lastTime).TotalMinutes;

        return minutesPassed >= rewardCooldownMinutes;
    }

    void SaveRewardTime()
    {
        PlayerPrefs.SetString(
            lastRewardKey,
            System.DateTime.Now.ToString()
        );

        PlayerPrefs.Save();
    }

    // =========================================================
    // FEEDBACK
    // =========================================================
    private IEnumerator ShowFeedback(string message)
    {
        if (feedbackPanel == null || feedbackText == null)
            yield break;

        feedbackPanel.SetActive(true);
        feedbackText.text = message;

        yield return new WaitForSeconds(3f);

        feedbackPanel.SetActive(false);
    }

    void UpdateAudioButtonTexts()
{
    if (musicButtonText != null)
    {
        bool musicOn = AudioSettingsManager.Instance == null ||
                       AudioSettingsManager.Instance.MusicEnabled;

        musicButtonText.text = musicOn ? "Music: ON" : "Music: OFF";
    }

    if (sfxButtonText != null)
    {
        bool sfxOn = AudioSettingsManager.Instance == null ||
                     AudioSettingsManager.Instance.SfxEnabled;

        sfxButtonText.text = sfxOn ? "SFX: ON" : "SFX: OFF";
    }
}


public void ToggleMusic()
{
    if (AudioSettingsManager.Instance != null)
    {
        AudioSettingsManager.Instance.ToggleMusic();
    }

    if (MusicManager.Instance != null)
        MusicManager.Instance.ApplyMusicSetting();

    if (AudioManager.Instance != null)
        AudioManager.Instance.ApplyAudioSettings();

    RunBGMController runBgm = FindFirstObjectByType<RunBGMController>();
    if (runBgm != null)
        runBgm.ApplyMusicSetting();

    UpdateAudioButtonTexts();
}
public void ToggleSfx()
{
    if (AudioSettingsManager.Instance != null)
    {
        AudioSettingsManager.Instance.ToggleSfx();
    }

    UpdateAudioButtonTexts();
}

    
}