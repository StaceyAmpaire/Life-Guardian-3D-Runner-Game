using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject tutorialPanel;

    // Static variables persist as long as the game is running, even if the scene changes
    private static bool tutorialShownThisSession = false;

    void Start()
    {
        // DEVELOPER MODE: Shows every time you launch the game, but not when returning to menu
        if (!tutorialShownThisSession)
        {
            tutorialPanel.SetActive(true);
            tutorialShownThisSession = true;
        }
        else
        {
            tutorialPanel.SetActive(false);
        }

        /* 
        // PLAYER MODE: Uncomment this and delete the "DEVELOPER MODE" block above 
        // to make it show only once ever for the player.
        
        if (PlayerPrefs.GetInt("HasSeenTutorial", 0) == 0)
        {
            tutorialPanel.SetActive(true);
            PlayerPrefs.SetInt("HasSeenTutorial", 1);
            PlayerPrefs.Save();
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
        */
    }

    public void OpenTutorial() => tutorialPanel.SetActive(true);
    public void CloseTutorial() => tutorialPanel.SetActive(false);
}
