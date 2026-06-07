using UnityEngine;

public class PlayerRoadMovement : MonoBehaviour
{
    [Header("Movement")]
    public float currentSpeed = 3f;
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float maxSpeed = 12f;
    public float sideSpeed = 6f;

    public float leftLimit = 392f;
    public float rightLimit = 405f;
    public float roadY = 0.05f;

    [Header("Health")]
    public float healthPercent = 40f;
    public float walkPercent = 45f;
    public float deathPercent = 30f;

    [Header("Body Size")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 hugeScale = new Vector3(2f, 2f, 2f);
    public float scaleSpeed = 2f;

    [Header("Clinic Bonus")]
    public int clinicBonusPoints = 500;

    [Header("Animator")]
    public Animator animator;

    [Header("Animation State Names")]
    public string walkingStateName = "Walking";
    public string runningStateName = "Running";
    public string dyingStateName = "Walking To Dying";

    [Header("UI")]
    public GameObject infoPanel;
    public GameObject gameOverPanel;

    private bool reachedClinic = false;
    private bool stopped = false;
    [Header("Hospital Rule")]
    public float hospitalDeadline = 60f; // player must reach hospital within 1 minute
    private float gameTimer = 0f;

    void Start()
    {
        currentSpeed = walkSpeed;
        transform.localScale = hugeScale;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        PlayWalking();

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
void Update()
    {
        if (stopped) return;

        gameTimer += Time.deltaTime;

        if (!reachedClinic && gameTimer >= hospitalDeadline)
        {
            FailToReachHospital();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;

        pos.z += currentSpeed * Time.deltaTime;
        pos.x += horizontal * sideSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.y = roadY;

        transform.position = pos;

        UpdateBodySize();
    }

    void UpdateBodySize()
    {
        Vector3 targetScale;

        if (healthPercent <= 40f)
        {
            targetScale = hugeScale;
        }
        else
        {
            float t = (healthPercent - 40f) / 60f;
            targetScale = Vector3.Lerp(hugeScale, normalScale, t);
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    public void ReachClinic()
    {
        if (reachedClinic || stopped) return;

        reachedClinic = true;

        healthPercent += 10f;
        healthPercent = Mathf.Clamp(healthPercent, 0f, 100f);

        TopBarManager topBar = FindObjectOfType<TopBarManager>();

        if (topBar != null)
            topBar.AddScore(clinicBonusPoints);

        PlayRunning();
    }

    public void TakeHealthyChoice(float percentGain)
    {
        if (stopped) return;

        healthPercent += percentGain;
        healthPercent = Mathf.Clamp(healthPercent, 0f, 100f);

        if (reachedClinic && healthPercent >= walkPercent)
        {
            PlayRunning();
        }
    }

    public void TakeWrongChoice(float percentLoss)
    {
        if (stopped) return;

        healthPercent -= percentLoss;
        healthPercent = Mathf.Clamp(healthPercent, 0f, 100f);

        if (healthPercent < deathPercent)
        {
            PlayDying();
            return;
        }

        if (reachedClinic && healthPercent < walkPercent)
        {
            PlayWalking();
            return;
        }

        currentSpeed -= 1.5f;
        currentSpeed = Mathf.Clamp(currentSpeed, walkSpeed, maxSpeed);
    }

    void PlayRunning()
    {
        currentSpeed = runSpeed;

        if (animator == null) return;

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        animator.SetBool("isWeak", false);

        animator.Play(runningStateName);
    }

    void PlayWalking()
    {
        currentSpeed = walkSpeed;

        if (animator == null) return;

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWeak", false);

        animator.Play(walkingStateName);
    }

    void PlayDying()
    {
        stopped = true;
        currentSpeed = 0f;

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWeak", true);

            animator.Play(dyingStateName);
        }

        if (infoPanel != null)
            infoPanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void StopPlayer()
        {
            currentSpeed = 0f;

            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isWeak", false);
                animator.speed = 0f;
            }

            enabled = false;
        }
    
    void FailToReachHospital()
        {
            stopped = true;
            currentSpeed = 0f;

            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isWeak", true);
                animator.Play(dyingStateName);
            }

            if (infoPanel != null)
                infoPanel.SetActive(true);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }
}