using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TutorialManager tutorialManager; // Reference to the new TutorialManager script
    public UserProfileManager userProfileManager; // New: Reference to UserProfileManager

    void Start()
{
    if (!TutorialManager.HasSeenTutorial())
    {
        tutorialManager.ShowTutorial();
    }
    else
    {
        tutorialManager.tutorialPanel.SetActive(false);
    }

    if (UserProfileManager.Instance != null &&
        string.IsNullOrEmpty(UserProfileManager.Instance.PlayerName))
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
