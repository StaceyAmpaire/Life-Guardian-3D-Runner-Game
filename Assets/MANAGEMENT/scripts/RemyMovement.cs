using UnityEngine;

public class RemyMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneSpeed = 15f; // Incremented slightly so lane shifts feel snappy on mobile

    public float roadCenterX = 400f;
    public float roadWidth = 40f;

    private float minX;
    private float maxX;
    private float _originalY;

    // --- Lane Setup ---
    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private float[] laneXPositions = new float[3];

    // --- Mobile Touch Detection Variable System ---
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private const float MIN_SWIPE_DISTANCE = 40f; // Distance in pixels required to confirm a swipe

    void Start()
    {
        _originalY = transform.position.y;

        // Calculate specific X coordinate locations for the 3 distinct lanes
        float halfWidth = roadWidth / 2f;
        laneXPositions[0] = roadCenterX - halfWidth; // Left Lane (380f)
        laneXPositions[1] = roadCenterX;             // Center Lane (400f)
        laneXPositions[2] = roadCenterX + halfWidth; // Right Lane (420f)

        minX = laneXPositions[0];
        maxX = laneXPositions[2];
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // 1. Maintain Continuous Forward Progress
        pos.z += forwardSpeed * Time.deltaTime;

        // 2. Handle Inputs (Merges PC testing and live mobile touch tracking structures)
        HandleKeyboardInput();
        HandleMobileSwipeInput();

        // 3. Smoothly Interpolate to Target Position
        float targetX = laneXPositions[currentLane];
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneSpeed * Time.deltaTime);

        // 4. Enforce Boundary Clamping 
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = _originalY;

        transform.position = pos;
    }

    // --- PC CONTROLS (Kept for testing inside Unity Editor) ---
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ShiftLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ShiftLane(1);
        }
    }

    // --- MOBILE TOUCH CONTROLS ---
    private void HandleMobileSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (!isSwiping) return;

                    Vector2 direction = touch.position - touchStartPos;

                    // Confirm the player swiped far enough horizontally
                    if (Mathf.Abs(direction.x) > MIN_SWIPE_DISTANCE)
                    {
                        if (direction.x > 0)
                        {
                            ShiftLane(1); // Swipe Right
                        }
                        else
                        {
                            ShiftLane(-1); // Swipe Left
                        }
                        isSwiping = false; // Kill swipe sequence tracking until next single discrete touch
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isSwiping = false;
                    break;
            }
        }
    }

    // --- LANE SHIFT LOGIC ---
    private void ShiftLane(int direction)
    {
        // Clamp lane changes strictly between indices 0 and 2
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }
}
