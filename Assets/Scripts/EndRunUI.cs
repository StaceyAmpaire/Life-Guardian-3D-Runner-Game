using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndRunUI : MonoBehaviour
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

        // ❌ HIDE EVERYTHING ELSE
        if (hudUI != null)
            hudUI.SetActive(false);

        if (pauseUI != null)
            pauseUI.SetActive(false);

        // ✅ SHOW END SCREEN ONLY
        if (endUI != null)
            endUI.SetActive(true);

        pointsText.text = "Points: " + MasterInfo.dewCount;
        healthyText.text = "Healthy Choices: " + MasterInfo.healthyCount;
        unhealthyText.text = "Unhealthy Choices: " + MasterInfo.unhealthyCount;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Run");
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