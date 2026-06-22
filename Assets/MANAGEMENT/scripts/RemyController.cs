using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RemyController : MonoBehaviour
{
    [Header("Score Settings")]
   public int maxScore = 100;
public int goodChoicePoints = 5;
public int badChoicePoints = 5;
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
public float laneDistance = 12f;
public float laneChangeSpeed = 2f;
public float centerOffset = 400f;

private int currentLane = 1;
private float targetX;
private float xVelocity;
private float currentX;
[Header("Swipe Controls")]
public float minSwipeDistance = 50f;

private Vector2 touchStartPos;
private bool isSwiping = false;
    private int _score;

// Management-only health
private int mgtHealth = 20;
    private bool _isDead = false;
    private float _currentSpeed;
    public EndGame endGame;
   

    public float HealthPct => mgtHealth;

    void Start()
    {
      

        // Initialize score to 20% of max (1000 points if maxScore is 5000)
        if (animator == null)
{
    animator = GetComponentInChildren<Animator>(true);
}
       _score = mgtHealth;

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
        targetX = centerOffset;
currentX = transform.position.x;
    }

    void Update()
{

    if (_isDead)
        return;

    HandleInput();
    MovePlayer();

    transform.rotation = Quaternion.identity;
    DebugAnimationState();
}

private void HandleInput()
{
    // Keyboard controls
    if (Input.GetKeyDown(KeyCode.LeftArrow) ||
        Input.GetKeyDown(KeyCode.A))
    {
        ChangeLane(-1);
    }

    if (Input.GetKeyDown(KeyCode.RightArrow) ||
        Input.GetKeyDown(KeyCode.D))
    {
        ChangeLane(1);
    }

    // Mobile swipe controls
    HandleSwipeInput();
}

private void HandleSwipeInput()
{
    if (Input.touchCount <= 0)
        return;

    Touch touch = Input.GetTouch(0);

    switch (touch.phase)
    {
        case TouchPhase.Began:
            touchStartPos = touch.position;
            isSwiping = true;
            break;

        case TouchPhase.Moved:

            if (!isSwiping)
                return;

            Vector2 swipeDelta = touch.position - touchStartPos;

            if (Mathf.Abs(swipeDelta.x) > minSwipeDistance)
            {
                if (swipeDelta.x > 0)
                {
                    ChangeLane(1);   // swipe right
                }
                else
                {
                    ChangeLane(-1);  // swipe left
                }

                isSwiping = false;
            }

            break;

        case TouchPhase.Ended:
        case TouchPhase.Canceled:
            isSwiping = false;
            break;
    }
}

private void ChangeLane(int direction)
{
    currentLane = Mathf.Clamp(currentLane + direction, 0, 2);

    targetX =
        (currentLane - 1) * laneDistance +
        centerOffset;
}
private void MovePlayer()
{
    if (_isDead) return;
    // Smooth lane movement
    currentX = Mathf.Lerp(
        currentX,
        targetX,
        laneChangeSpeed * Time.deltaTime);

    Vector3 pos = transform.position;

    pos.x = currentX;
    pos.z += _currentSpeed * Time.deltaTime;

    transform.position = pos;
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

    mgtHealth = Mathf.Clamp(mgtHealth + amount, 0, 100);
    _score = mgtHealth;

    // Update main game life using MasterInfo helper
    MasterInfo.SetTreeLife(MasterInfo.treeLife + amount);
    MasterInfo.SaveData();

    UpdateState();
    UpdateUI();
}

  public void RemovePoints(int amount)
{
    if (_isDead) return;

    mgtHealth = Mathf.Clamp(mgtHealth - amount, 0, 100);
    _score = mgtHealth;

    // Update main game life using MasterInfo helper
    MasterInfo.SetTreeLife(MasterInfo.treeLife - amount);
    MasterInfo.SaveData();

    UpdateState();
    UpdateUI();
}

 private void UpdateState()
{
     if (_isDead) return;
    float hp = HealthPct;

    SetAllAnimationsFalse();

    

    if (hp <= 10f)
    {
        Die();
        return;
    }

    if (hp < 40f)
    {
        _currentSpeed = normalWalkSpeed;
        animator.CrossFade("Walking", 0.1f);
        UpdateStateText("Walking");
    }
    else if (hp < 80f)
    {
        _currentSpeed = slowRunSpeed;
        animator.CrossFade("Slow Run", 0.1f);
        UpdateStateText("Gaining");
    }
    else
    {
        _currentSpeed = runSpeed;
        animator.CrossFade("Medium Run", 0.1f);
        UpdateStateText("Running");
    }
    Debug.Log("Health = " + hp);
}

    private void Die()
{
    if (_isDead) return;

    _isDead = true;
    _currentSpeed = 0f;

    SetAllAnimationsFalse();

    if (animator != null)
    {
        animator.SetBool("isDead", true);
        animator.CrossFade("Dying", 0.1f); // 🔥 FORCE PLAY
    }

    UpdateStateText("Dead");

    StartCoroutine(StopGameAfterDeathAnimation());
}

        private IEnumerator StopGameAfterDeathAnimation()
            {
                yield return new WaitForSeconds(4f);

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
    get { return mgtHealth; }
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
    private void DebugAnimationState()
{
    if (animator == null) return;

    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

    if (state.IsName("Running"))
        Debug.Log("🟢 Currently: Running");
    else if (state.IsName("Walking"))
        Debug.Log("🟡 Currently: Walking");
    else if (state.IsName("Slow Run"))
        Debug.Log("🟠 Currently: Slow Run");
    else if (state.IsName("Medium Run"))
        Debug.Log("🔵 Currently: Medium Run");
   else if (state.IsName("Dying"))
    Debug.Log("🔴 Currently: Dying");
    else
        Debug.Log("⚠️ Unknown Animation State: " + state.fullPathHash);
}

}