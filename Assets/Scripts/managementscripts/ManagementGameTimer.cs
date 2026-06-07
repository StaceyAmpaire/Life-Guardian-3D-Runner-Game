using UnityEngine;
using TMPro;

public class ManagementGameTimer : MonoBehaviour
{
    [Header("Timer")]
    public float gameTime = 180f; // 3 minutes

    [Header("UI")]
    public TMP_Text timerText;

    [Header("Panels")]
    public GameObject gameOverPanel;

    private float remainingTime;
    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;

        remainingTime = gameTime;
        gameEnded = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateTimerUI();
    }

    void Update()
    {
        if (gameEnded) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateTimerUI();
            EndGame();
            return;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        float safeTime = Mathf.Max(0f, remainingTime);

        int minutes = Mathf.FloorToInt(safeTime / 60f);
        int seconds = Mathf.FloorToInt(safeTime % 60f);

        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    void EndGame()
        {
            if (gameEnded) return;

            gameEnded = true;

            PlayerRoadMovement player = FindObjectOfType<PlayerRoadMovement>();

            if (player != null)
            {
                player.StopPlayer();
            }

            // SHOW FINAL FOOD REPORT
            FoodCollectionManager foodReport =
                FindObjectOfType<FoodCollectionManager>();

            if (foodReport != null)
            {
                foodReport.ShowFinalFoodReport();
            }

            // OPTIONAL GAME OVER PANEL
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f;

            Debug.Log("TIME FINISHED - GAME STOPPED");
        }
    }