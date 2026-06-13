using UnityEngine;

public class RemyMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneSpeed = 8f;

    public float minX = 392f;
    public float maxX = 408f;
    
    private float _originalY; // Store the original height

    void Start()
    {
        // Store the character's initial Y position (height)
        _originalY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Forward movement (Z axis)
        pos.z += forwardSpeed * Time.deltaTime;

        // Lane movement (X axis)
        float horizontal = Input.GetAxis("Horizontal");
        pos.x += horizontal * laneSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        // CRITICAL FIX: Maintain the original Y position (prevent sinking into ground)
        pos.y = _originalY;

        // Apply the position
        transform.position = pos;
    }

    
}