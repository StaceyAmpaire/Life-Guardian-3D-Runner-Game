using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TutorialManager tutorialManager; // Reference to the new TutorialManager script

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
    }

    // This method will be called by a UI button to manually open the tutorial
    public void OpenTutorial()
    {
        tutorialManager.ShowTutorial();
    }

    // This method will be called by the 'I Understand' button in the TutorialManager
    // No longer needed here as TutorialManager handles closing itself.
    // public void CloseTutorial() => tutorialManager.tutorialPanel.SetActive(false);
}
