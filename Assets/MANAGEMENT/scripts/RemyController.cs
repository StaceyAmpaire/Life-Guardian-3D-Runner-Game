using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RemyController : MonoBehaviour
{
    [Header("Score Settings")]
    public int maxScore = 5000;
    public int goodChoicePoints = 250;
    public int badChoicePoints = 250;

    [Header("Movement Speeds")]
    public float weakWalkSpeed = 1.5f;
    public float normalWalkSpeed = 4f;
    public float slowRunSpeed = 5.5f;
    public float runSpeed = 7f;
    public float fastRunSpeed = 9f;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text stateText;
    public Slider healthBar;
    public GameObject gameOverPanel;
    public TopBarPanel topBarPanel;

    [Header("Animator")]
    public Animator animator;

    [Header("Lane Movement")]
    public float laneSpeed = 8f;
    public float minX = 388f;
    public float maxX = 412f;

    private int _score;
    private bool _isDead = false;
    private float _currentSpeed;
    public EndGame endGame;
   

    public float HealthPct => (float)_score / maxScore * 100f;

    void Start()
    {
        // Initialize score to 20% of max (1000 points if maxScore is 5000)
        _score = Mathf.RoundToInt(maxScore * 0.2f);

        // Get animator if not assigned
        if (animator == null)
            animator = GetComponent<Animator>();

    
        // Hide game over panel at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Ensure time scale is normal
        Time.timeScale = 1f;

        // Lock rotation on X and Z to prevent tilting
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        // Initialize state and UI
        UpdateState();
        UpdateUI();
    }

    void Update()
    {
        if (_isDead) return;

        // FORWARD movement - using world Z axis (camera independent)
        Vector3 movement = new Vector3(0f, 0f, _currentSpeed * Time.deltaTime);

        // LANE movement - horizontal on X axis
        float horizontal = Input.GetAxis("Horizontal");
        movement.x = horizontal * laneSpeed * Time.deltaTime;

        // Apply movement
        transform.position += movement;

        // Clamp X position
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        // Keep rotation fixed (no turning)
        transform.rotation = Quaternion.identity;
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
        Die();
        return;
    }

    if (hp < 50f)
    {
        _currentSpeed = normalWalkSpeed;

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.CrossFade("Walking", 0.1f);
        }

        UpdateStateText("Walking");
    }
    else if (hp < 75f)
    {
        _currentSpeed = slowRunSpeed;

        if (animator != null)
        {
            animator.SetBool("isGaining", true);
            animator.CrossFade("SlowRun", 0.1f);
        }

        UpdateStateText("Gaining");
    }
    else
    {
        _currentSpeed = runSpeed;

        if (animator != null)
        {
            animator.SetBool("isRunning", true);
            animator.CrossFade("Running", 0.1f);
        }

        UpdateStateText("Running");
    }
}

    private void Die()
        {
            _isDead = true;
            _currentSpeed = 0f;

            SetAllAnimationsFalse();

            if (animator != null)
                animator.SetBool("isDead", true);

            UpdateStateText("Dead");

            StartCoroutine(StopGameAfterDeathAnimation());
        }

        private IEnumerator StopGameAfterDeathAnimation()
            {
                yield return new WaitForSeconds(2f);

                TriggerGameOver();

                Time.timeScale = 0f;
            }
    private void SetAllAnimationsFalse()
        {
            if (animator == null) return;

            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isGaining", false);
            animator.SetBool("isDead", false);
        }
    private void TriggerGameOver()
        {
            string message = GetPerformanceMessage(_score);

            if (endGame != null)
            {
                endGame.ShowGameOver(message);
            }
            else
            {
                Debug.LogError("EndGame is not assigned in RemyController Inspector!");
            }
            
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

        // Update score text
        if (scoreText != null)
            scoreText.text = "Score: " + _score;

        // Update health text
        if (healthText != null)
            healthText.text = "Health: " + hp.ToString("F0") + "%";

        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = hp / 100f;

            // Change health bar color based on health percentage
            if (healthBar.fillRect != null)
            {
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

        // Update top bar panel if it exists
        if (topBarPanel != null)
        {
            topBarPanel.UpdateScore(_score);
            topBarPanel.UpdateHealth(_score, maxScore);
        }
    }

    public void ShowChoiceMessage(string message)
    {
        if (topBarPanel != null)
            topBarPanel.SetQuestion(message);
    }

    
    public int CurrentScore
    {
        get { return _score; }
    }


public string GetPerformanceMessage(int score)
    {
        float percentage = ((float)score / maxScore) * 100f;

        if (percentage <= 10f)
            return "Remy's health became critical due to unhealthy lifestyle choices.";

        if (percentage < 50f)
            return "Remy struggled to maintain good health. More healthy habits are needed.";

        if (percentage < 75f)
            return "Good effort! Remy improved health through better lifestyle choices.";

        return "Excellent! Remy maintained a healthy lifestyle and made outstanding choices.";
    }
}