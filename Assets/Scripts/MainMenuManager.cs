using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TutorialManager tutorialManager; // Reference to the new TutorialManager script
    public UserProfileManager userProfileManager; // New: Reference to UserProfileManager

    void Start()
    {
        // Check if the tutorial has been shown this session using MasterInfo's flag
        if (!MasterInfo.tutorialShownThisSession)
        {
            tutorialManager.ShowTutorial();
            MasterInfo.tutorialShownThisSession = true; // Set the flag so it doesn't show again this session
        }
        else
        {
            // Ensure the tutorial panel is hidden if it has already been shown this session
            tutorialManager.tutorialPanel.SetActive(false);
        }

        // New: Show name input if player name is not set
        if (string.IsNullOrEmpty(UserProfileManager.Instance.PlayerName))
        {
            userProfileManager.ShowNameInputPanel();
        }
    }

    // This method will be called by a UI button to manually open the tutorial
    public void OpenTutorial()
    {
        tutorialManager.ShowTutorial();
    }

    // New: Call this from your Settings button
    public void OnSettingsButtonClicked()
    {
        userProfileManager.ShowNameInputPanel();
    }
}
