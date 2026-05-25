using UnityEngine;
using TMPro;
using System;

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject nameInputPanel; // The popup panel for name input
    public TMP_InputField nameInputField; // The input field for the player's name
    public TMP_Text playerNameDisplay; // TextMeshPro object to display player name on cobblestone
    public TMP_Text setNamePrompt; // TextMeshPro object to display "Set Name" initially
    public GameObject dailyRewardButton; // New: Button to claim daily reward
    public TMP_Text dailyRewardButtonText; // New: Text on the daily reward button

    [Header("Daily Reward Settings")]
    public int dailyRewardAmount = 10; // Amount of Healing Dew for daily login
    [Tooltip("Set to 0 for daily, 1 for every login (for testing).")]
    public int dailyRewardFrequencyDays = 1; // How often the reward can be claimed (in days)

    private const string PlayerNameKey = "PlayerName";
    private const string LastLoginDateKey = "LastLoginDate";
    private const string DailyRewardClaimedTodayKey = "DailyRewardClaimedToday"; // New: To track if claimed today

    public string PlayerName { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadPlayerProfile();
        CheckDailyRewardStatus(); // New: Check status at start
        UpdatePlayerNameDisplay();
    }

    public void ShowNameInputPanel()
    {
        nameInputPanel.SetActive(true);
        if (nameInputField != null) nameInputField.text = PlayerName;
        CheckDailyRewardStatus(); // Update button state when panel is shown
    }

    public void HideNameInputPanel()
    {
        nameInputPanel.SetActive(false);
    }

    public void SavePlayerName()
    {
        if (nameInputField != null && !string.IsNullOrWhiteSpace(nameInputField.text))
        {
            PlayerName = nameInputField.text.Trim();
            PlayerPrefs.SetString(PlayerNameKey, PlayerName);
            PlayerPrefs.Save();
            MasterInfo.SetPlayerName(PlayerName); // Update MasterInfo
            UpdatePlayerNameDisplay();
            HideNameInputPanel();
        }
        else
        {
            Debug.LogWarning("Player name cannot be empty.");
        }
    }

    private void LoadPlayerProfile()
    {
        PlayerName = PlayerPrefs.GetString(PlayerNameKey, "");
    }

    public void UpdatePlayerNameDisplay()
    {
        if (playerNameDisplay != null)
        {
            if (string.IsNullOrEmpty(PlayerName))
            {
                playerNameDisplay.gameObject.SetActive(false);
                if (setNamePrompt != null) setNamePrompt.gameObject.SetActive(true);
            }
            else
            {
                playerNameDisplay.text = PlayerName;
                playerNameDisplay.gameObject.SetActive(true);
                if (setNamePrompt != null) setNamePrompt.gameObject.SetActive(false);
            }
        }
        else if (setNamePrompt != null && string.IsNullOrEmpty(PlayerName))
        {
             setNamePrompt.gameObject.SetActive(true);
        }
    }

    private void CheckDailyRewardStatus()
    {
        if (dailyRewardButton == null) return;

        string lastLoginDateString = PlayerPrefs.GetString(LastLoginDateKey, string.Empty);
        DateTime lastLoginDate;
        bool claimedToday = PlayerPrefs.GetInt(DailyRewardClaimedTodayKey, 0) == 1;

        if (DateTime.TryParse(lastLoginDateString, out lastLoginDate))
        {
            // For testing: if dailyRewardFrequencyDays is 0, always enable reward
            if (dailyRewardFrequencyDays == 0)
            {
                dailyRewardButton.SetActive(true);
                if (dailyRewardButtonText != null) dailyRewardButtonText.text = $"Claim {dailyRewardAmount} Dew (Test)";
            }
            else if ((DateTime.Now - lastLoginDate).TotalDays >= dailyRewardFrequencyDays && !claimedToday)
            {
                dailyRewardButton.SetActive(true);
                if (dailyRewardButtonText != null) dailyRewardButtonText.text = $"Claim {dailyRewardAmount} Dew!";
            }
            else
            {
                dailyRewardButton.SetActive(false);
                if (dailyRewardButtonText != null) dailyRewardButtonText.text = "Claimed Today";
            }
        }
        else // First time login or invalid date string
        {
            dailyRewardButton.SetActive(true);
            if (dailyRewardButtonText != null) dailyRewardButtonText.text = $"Claim {dailyRewardAmount} Dew!";
        }
    }

    public void ClaimDailyReward()
    {
        if (MasterInfo.Instance != null)
        {
            MasterInfo.dewCount += dailyRewardAmount;
            MasterInfo.Instance.UpdateDewDisplay();
            Debug.Log($"Daily reward granted: {dailyRewardAmount} Healing Dew. Total: {MasterInfo.dewCount}");
        }
        PlayerPrefs.SetString(LastLoginDateKey, DateTime.Now.ToString());
        PlayerPrefs.SetInt(DailyRewardClaimedTodayKey, 1); // Mark as claimed for today
        PlayerPrefs.Save();
        CheckDailyRewardStatus(); // Update button state
    }

    // Call this from your Settings button or initial "Set Name" prompt
    public void OnSettingsButtonClicked()
    {
        ShowNameInputPanel();
    }

    // New: Call this when the game starts or returns to Main Menu to reset daily claim status if a new day
    public void ResetDailyClaimStatus()
    {
        string lastLoginDateString = PlayerPrefs.GetString(LastLoginDateKey, string.Empty);
        DateTime lastLoginDate;

        if (DateTime.TryParse(lastLoginDateString, out lastLoginDate))
        {
            if (DateTime.Now.Date > lastLoginDate.Date)
            {
                PlayerPrefs.SetInt(DailyRewardClaimedTodayKey, 0); // Reset for new day
                PlayerPrefs.Save();
            }
        }
    }
}
