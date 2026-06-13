using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndActivitiesUI : MonoBehaviour
{
    public GameObject endUI;

    public GameObject hudUI;
    public GameObject pauseUI;

    public TMP_Text pointsText;
    public TMP_Text healthyText;
    public TMP_Text unhealthyText;

    public void ShowPopup()
    {
        Time.timeScale = 0f;

        // Hide gameplay UI
        if (hudUI != null)
            hudUI.SetActive(false);

        if (pauseUI != null)
            pauseUI.SetActive(false);

        // Show end screen
        if (endUI != null)
            endUI.SetActive(true);

        pointsText.text =
            "Points: " + MasterInfo.dewCount;

        healthyText.text =
            "Healthy Activities: " +
            MasterInfo.healthyActivityCount;

        unhealthyText.text =
            "Unhealthy Activities: " +
            MasterInfo.unhealthyActivityCount;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        // Reset activity counters
        MasterInfo.healthyActivityCount = 0;
        MasterInfo.unhealthyActivityCount = 0;

        SceneManager.LoadScene("Run_Activities");
    }

    public void GoMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseToLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelectScene");
    }
}