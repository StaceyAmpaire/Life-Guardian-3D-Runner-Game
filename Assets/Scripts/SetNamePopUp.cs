using UnityEngine;
using TMPro;

public class PopupUI : MonoBehaviour
{
    [Header("Popup")]
    public GameObject popupPanel;

    [Header("Player Name")]
    public TMP_InputField nameInputField;
    public TMP_Text playerNameText;

    [Header("Food Logging")]
    public TMP_InputField foodInputField;

    [Header("Reward")]
    public int dewReward = 10;

    public float rewardCooldownHours = 5f;

    private string lastRewardKey = "LastFoodRewardTime";

    private void Start()
    {
#if UNITY_EDITOR

        // ONLY resets when pressing Play inside Unity editor
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerPrefs.DeleteKey("PlayerName");
            PlayerPrefs.DeleteKey(lastRewardKey);
        }

#endif

        LoadPlayerName();
    }

    public void OpenPopup()
    {
        popupPanel.SetActive(true);
        nameInputField.ActivateInputField();
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

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

    public void LogFood()
    {
        string enteredFood = foodInputField.text;

        if (string.IsNullOrWhiteSpace(enteredFood))
            return;

        if (CanClaimReward())
        {
            
            MasterInfo.totalDewCount += dewReward;

            if (MasterInfo.Instance != null)
            {
                MasterInfo.Instance.UpdateDewDisplay();
            }

            SaveRewardTime();

            Debug.Log("Reward Granted!");
        }
        else
        {
            Debug.Log("Reward already claimed.");
        }

        foodInputField.text = "";

        
       
    }

    void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName =
                PlayerPrefs.GetString("PlayerName");

            playerNameText.text = savedName;

            MasterInfo.SetPlayerName(savedName);

            nameInputField.text = savedName;
        }
        else
        {
            playerNameText.text = "Set Name";
        }
    }

    bool CanClaimReward()
    {
        if (!PlayerPrefs.HasKey(lastRewardKey))
            return true;

        string savedTime =
            PlayerPrefs.GetString(lastRewardKey);

        System.DateTime lastTime =
            System.DateTime.Parse(savedTime);

        double hoursPassed =
            (System.DateTime.Now - lastTime).TotalHours;

        return hoursPassed >= rewardCooldownHours;
    }

    void SaveRewardTime()
    {
        PlayerPrefs.SetString(
            lastRewardKey,
            System.DateTime.Now.ToString()
        );

        PlayerPrefs.Save();
    }
}