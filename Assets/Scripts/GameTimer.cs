using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TMP_Text timerText;

    private bool timerRunning = true;
    

    void Start()
{
    Time.timeScale = 1f;

    MasterInfo.dewCount = 0;
    MasterInfo.healthyCount = 0;
    MasterInfo.unhealthyCount = 0;

    // RESET STREAKS (THIS PART)
    MasterInfo.healthyStreak = 0;
    MasterInfo.unhealthyStreak = 0;

    UpdateTimerDisplay();
}

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            EndGame();
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time: " + seconds;
    }

    void EndGame()
    {
        FindFirstObjectByType<EndRunUI>().ShowPopup();
    }
}