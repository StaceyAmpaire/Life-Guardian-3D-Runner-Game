using UnityEngine;

public class RemyMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneSpeed = 8f;

    public float minX = 391f;
    public float maxX = 409f;

    void Update()
    {
        // Move forward
        transform.Translate(
            Vector3.forward * forwardSpeed * Time.deltaTime,
            Space.World
        );

        // Left / Right movement
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;

        pos.x += horizontal * laneSpeed * Time.deltaTime;

        // Prevent leaving road
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        transform.position = pos;
    }
}