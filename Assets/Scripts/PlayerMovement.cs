using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 4f; // How far apart the lanes are
    public float laneChangeSpeed = 15f; // How fast the player jumps to the next lane
    public float centerOffset = -6.0f; // Adjust this to align with your road's center

    [Header("Mobile Settings")]
    public float minSwipeDistance = 50f;

    private int currentLane = 1; // 0: Left, 1: Center, 2: Right
    private Vector2 startTouchPosition;
    private bool isSwiping = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Set initial position to the center lane immediately
        float targetX = (currentLane - 1) * laneDistance + centerOffset;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // KEYBOARD: A/D or Arrows
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
                ChangeLane(-1);
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
                ChangeLane(1);
        }

        // MOBILE: Swipe Detection
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
                        if (diff.x > 0) ChangeLane(1); // Swipe Right
                        else ChangeLane(-1); // Swipe Left
                    }
                    isSwiping = false; 
                }
            }
        }
        else { isSwiping = false; }
    }

    private void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    private void MovePlayer()
    {
        // Calculate where the player SHOULD be based on the lane
        float targetX = (currentLane - 1) * laneDistance + centerOffset;
        
        Vector3 moveVector = Vector3.zero;
        
        // 1. Forward Speed
        moveVector.z = forwardSpeed * Time.deltaTime;

        // 2. Smooth Lane Transition
        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneChangeSpeed);
        moveVector.x = newX - transform.position.x;

        // 3. Move the character
        if (controller != null) controller.Move(moveVector);
        else transform.Translate(moveVector, Space.World);
    }
}
