using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 4f;
    public float laneChangeSpeed = 15f;
    public float centerOffset = -6.0f;
    private float baseSpeed;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    // ✨ NEW: double jump system
    [Header("Jump Mechanics")]
    public int maxJumps = 2;
    private int jumpCount;

    [Header("Mobile Settings")]
    public float minSwipeDistance = 50f;

    private int currentLane = 1;
    private Vector2 startTouchPosition;
    private bool isSwiping = false;

    private CharacterController controller;
    private Vector3 velocity;

    private float targetX;
    private float xVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        baseSpeed = forwardSpeed;

        targetX = (currentLane - 1) * laneDistance + centerOffset;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        jumpCount = 0; // reset
    }

    void Update()
    {
        forwardSpeed =
Mathf.Clamp(
baseSpeed + MasterInfo.activityFitness,
7f,
13f);
        HandleInput();
        MovePlayer();

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
    }

    private void HandleInput()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
                ChangeLane(-1);

            if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
                ChangeLane(1);
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentTouchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!isSwiping)
            {
                startTouchPosition = currentTouchPos;
                isSwiping = true;
            }
            else
            {
                Vector2 diff = currentTouchPos - startTouchPosition;

                if (diff.magnitude > minSwipeDistance)
                {
                    if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                    {
                        if (diff.x > 0) ChangeLane(1);
                        else ChangeLane(-1);
                    }
                    else
                    {
                        if (diff.y > 0)
                        {
                            Jump(); // mobile jump
                        }
                    }

                    isSwiping = false;
                }
            }
        }
        else
        {
            isSwiping = false;
        }
    }

    private void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);

        targetX = (currentLane - 1) * laneDistance + centerOffset;
    }

    // ✨ UPDATED JUMP SYSTEM
    private void Jump()
    {
        if (jumpCount < maxJumps)
        {
            jumpCount++;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            Animator[] anims = GetComponentsInChildren<Animator>(true);

            foreach (Animator anim in anims)
{
    if (anim.gameObject.name == "Running (1)" ||
        anim.gameObject.name == "Running (2)")
    {
        anim.ResetTrigger("Jump");
        anim.SetTrigger("Jump");
    }
}
        }
    }

    private void MovePlayer()
    {
        bool isGrounded = controller.isGrounded;

        // reset jump counter when grounded
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float newX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,
            ref xVelocity,
            1f / laneChangeSpeed
        );

        Vector3 move = Vector3.zero;

        move.x = newX - transform.position.x;
        move.z = forwardSpeed * Time.deltaTime;

        controller.Move(move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}