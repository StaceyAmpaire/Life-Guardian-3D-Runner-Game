using UnityEngine;
using TMPro;

public class TopBarManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;

    public int maxScore = 5000;

    [Header("Starting Health")]
    public int startingHealthPercent = 20 ;

    public int score;

    void Start()
    {
        score = Mathf.RoundToInt((startingHealthPercent / 100f) * maxScore);
        UpdateTopBar();
    }

    public void AddScore(int points)
    {
        score += points;
        score = Mathf.Clamp(score, 0, maxScore);

        UpdateTopBar();
    }

    public int GetHealthPercent()
    {
        int healthPercent =
            Mathf.RoundToInt((score / (float)maxScore) * 100f);

        return Mathf.Clamp(healthPercent, 0, 100);
    }

    void UpdateTopBar()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (healthText != null)
            healthText.text = "Health: " + GetHealthPercent() + "%";
    }
}