using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopBarPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public TMP_Text questionText;
    public TMP_Text timerText;
    public Slider healthBar;
    public Button pauseButton;

    [Header("Timer")]
    public float gameTime = 90f; // 1.5 minutes
    private float _timeLeft;
    private bool _isPaused = false;
    private bool _gameEnded = false;
    public EndGame endGame;
    public RemyController remyController;
    public FinalChoicePanel finalChoicePanel;

    void Start()
    {
        _timeLeft = gameTime;

        if (questionText != null)
            questionText.text = "What do you choose?";

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        UpdateTimerUI();

        if (finalChoicePanel == null)
        finalChoicePanel = FindFirstObjectByType<FinalChoicePanel>();
    }

    void Update()
    {
        if (_isPaused || _gameEnded) return;

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0)
        {
            _timeLeft = 0;
            EndGame();
        }

        UpdateTimerUI();
    }

    public void UpdateHealth(int score, int maxScore)
    {
        float hp = (float)score / maxScore;
        int hpPercent = Mathf.RoundToInt(hp * 100f);

        if (healthBar != null)
            healthBar.value = hp;

        if (healthText != null)
            healthText.text = hpPercent + "%";
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void SetQuestion(string message)
    {
        if (questionText != null)
            questionText.text = message;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_timeLeft / 60f);
        int seconds = Mathf.FloorToInt(_timeLeft % 60f);

        if (timerText != null)
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void TogglePause()
    {
        Debug.Log("PAUSE BUTTON CLICKED");

        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;

        Debug.Log("Time Scale = " + Time.timeScale);
    }

    public void EndGame()
    {
        _gameEnded = true;

        if (endGame != null)
        {
            string message = remyController.GetPerformanceMessage(remyController.CurrentScore);
            endGame.ShowGameOver(message);
        }
        else
        {
            Debug.LogError("EndGame script is not assigned in TopBarPanel Inspector!");
        }

        if (finalChoicePanel != null)
        {
            finalChoicePanel.ShowFinalChoices();
        }
        else
        {
            Debug.LogWarning("FinalChoicePanel is not assigned!");
        }
    }
}