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
        
        // Clear current animations
        SetAllAnimationsFalse();

        // Check for death (health <= 10%)
        if (hp <= 10f)
        {
            Die();
            return;
        }

        // Determine movement state based on health percentage
        if (hp <= 30f)
        {
            _currentSpeed = weakWalkSpeed;
            if (animator != null) animator.SetBool("isWalking", true);
            UpdateStateText("Weak Walking");
        }
        else if (hp <= 50f)
        {
            _currentSpeed = normalWalkSpeed;
            if (animator != null) animator.SetBool("isWalking", true);
            UpdateStateText("Walking");
        }
        else if (hp <= 70f)
        {
            _currentSpeed = slowRunSpeed;
            if (animator != null) animator.SetBool("isRunning", true);
            UpdateStateText("Slow Run");
        }
        else if (hp <= 85f)
        {
            _currentSpeed = runSpeed;
            if (animator != null) animator.SetBool("isRunning", true);
            UpdateStateText("Running");
        }
        else // hp > 85%
        {
            _currentSpeed = fastRunSpeed;
            if (animator != null) animator.SetBool("isRunning", true);
            UpdateStateText("Fast Run");
        }
    }

    private void Die()
    {
        _isDead = true;
        _currentSpeed = 0f;

        if (animator != null)
            animator.SetBool("isDead", true);

        UpdateStateText("Dead");
        TriggerGameOver();
    }

    private void SetAllAnimationsFalse()
    {
        if (animator == null) return;

        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", false);
    }

    private void TriggerGameOver()
    {
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
}