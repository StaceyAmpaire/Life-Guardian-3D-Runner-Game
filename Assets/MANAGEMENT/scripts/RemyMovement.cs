using UnityEngine;

public class RemyMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneSpeed = 8f;

    public float minX = 388f;
    public float maxX = 412f;

    private float _originalY;

    void Start()
    {
        _originalY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        pos.z += forwardSpeed * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        pos.x += horizontal * laneSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        pos.y = _originalY;

        transform.position = pos;
    }
}