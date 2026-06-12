using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RemyController : MonoBehaviour
{
    [Header("Score Settings")]
    public int maxScore = 5000;
    public int goodChoicePoints = 250;
    public int badChoicePoints = 250;

    [Header("Movement Speeds")]
    public float runSpeed = 8f;
    public float walkSpeed = 4f;
    public float weakSpeed = 1.5f;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text stateText;
    public Slider healthBar;
    public GameObject gameOverPanel;

    [Header("Animator")]
    public Animator animator;

    private int _score = 1000; // 20% health
    private bool _isDead = false;
    private float _currentSpeed;

    public float HealthPct => (float)_score / maxScore * 100f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateState();
        UpdateUI();
    }

    void Update()
    {
        if (_isDead) return;

        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
    }

    public void GoodChoice()
    {
        AddPoints(goodChoicePoints);
    }

    public void BadChoice()
    {
        RemovePoints(badChoicePoints);
    }

    public void AddPoints(int amount)
    {
        if (_isDead) return;

        _score = Mathf.Min(_score + amount, maxScore);

        UpdateState();
        UpdateUI();
    }

    public void RemovePoints(int amount)
    {
        if (_isDead) return;

        _score = Mathf.Max(_score - amount, 0);

        UpdateState();
        UpdateUI();
    }

    private void UpdateState()
    {
        float hp = HealthPct;

        SetAllAnimationsFalse();

        if (hp <= 10f)
        {
            _isDead = true;
            _currentSpeed = 0f;

            if (animator != null)
                animator.SetBool("isDead", true);

            TriggerGameOver();
            return;
        }

        if (hp <= 30f)
        {
            _currentSpeed = weakSpeed;

            if (animator != null)
                animator.SetBool("isWeak", true);

            UpdateStateText("Weak");
            return;
        }

        if (hp <= 60f)
        {
            _currentSpeed = walkSpeed;

            if (animator != null)
                animator.SetBool("isWalking", true);

            UpdateStateText("Walking");
            return;
        }

        _currentSpeed = runSpeed;

        if (animator != null)
            animator.SetBool("isRunning", true);

        UpdateStateText("Running");
    }

    private void SetAllAnimationsFalse()
    {
        if (animator == null) return;

        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isWeak", false);
        animator.SetBool("isDead", false);
    }

    private void TriggerGameOver()
    {
        UpdateStateText("Dead");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Game Over! Remy died.");
    }

    private void UpdateStateText(string state)
    {
        if (stateText != null)
            stateText.text = state;
    }

    private void UpdateUI()
    {
        float hp = HealthPct;

        if (scoreText != null)
            scoreText.text = "Score: " + _score;

        if (healthText != null)
            healthText.text = "Health: " + hp.ToString("F0") + "%";

        if (healthBar != null)
        {
            healthBar.value = hp / 100f;

            Image fill = healthBar.fillRect.GetComponent<Image>();

            if (fill != null)
            {
                if (hp > 60f)
                    fill.color = Color.green;
                else if (hp > 30f)
                    fill.color = Color.yellow;
                else
                    fill.color = Color.red;
            }
        }
    }
}