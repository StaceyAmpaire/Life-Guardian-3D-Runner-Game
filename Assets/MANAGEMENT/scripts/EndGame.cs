using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Add this for UI Slider

public class EndGame : MonoBehaviour
{
    public GameObject endGamePanel;
    public GameObject hudPanel;

    public TMP_Text scorePanelText;
    public TMP_Text finalScoreText;
    public TMP_Text messageText;

    public Slider healthBar; // Add this reference
    public TMP_Text healthPercentageText; // Optional: add this to show percentage text

    private bool gameOverShown = false;
    public RemyController remyController;

    public void ShowGameOver(string message)
        {
            if (gameOverShown) return;

            gameOverShown = true;

            Time.timeScale = 0f;

            if (hudPanel != null)
                hudPanel.SetActive(false);

            if (endGamePanel != null)
                endGamePanel.SetActive(true);

            if (scorePanelText != null && finalScoreText != null)
                finalScoreText.text = scorePanelText.text.Replace("Score:", "Final Score:");

            if (messageText != null)
                messageText.text = message;

            // Display the health bar with Remy's final health percentage
            if (remyController != null && healthBar != null)
            {
                float healthPct = remyController.HealthPct;
                healthBar.value = healthPct / 100f;
                
                // Optional: display percentage text
                if (healthPercentageText != null)
                    healthPercentageText.text = $"Health: {healthPct:F0}%";
                
                // Optional: change health bar color based on percentage
                if (healthBar.fillRect != null)
                {
                    Image fill = healthBar.fillRect.GetComponent<Image>();
                    if (fill != null)
                    {
                        if (healthPct > 60f)
                            fill.color = Color.green;
                        else if (healthPct > 30f)
                            fill.color = Color.yellow;
                        else
                            fill.color = Color.red;
                    }
                }
            }
            if (AchievementManager.Instance != null)
{
    AchievementManager.Instance.MarkFirstRunComplete();
}
        }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}