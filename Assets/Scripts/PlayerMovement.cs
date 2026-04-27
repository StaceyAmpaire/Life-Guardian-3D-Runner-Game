using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 6f;
    public float horizontalSpeed = 3f;

    public float leftLimit = -13.3f;
    public float rightLimit = 1.4f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > leftLimit)
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < rightLimit)
                transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed);
        }
    }
}