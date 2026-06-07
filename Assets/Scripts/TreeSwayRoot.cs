using UnityEngine;

public class TreeSwayRoot : MonoBehaviour
{
    public float swayAmountZ = 1.5f; // side to side
    public float swayAmountX = 0.6f; // forward/back (YES this is what you asked)
    public float swayAmountY = 0.2f; // optional twist

    public float swaySpeed = 0.8f;

    private float offset;

    void Start()
    {
        offset = Random.Range(0f, 10f);
    }

    void Update()
    {
        float t = Time.time * swaySpeed + offset;

        float z = Mathf.Sin(t) * swayAmountZ;        // side sway
        float x = Mathf.Cos(t * 0.8f) * swayAmountX; // TOP ↔ BOTTOM feeling (tilt forward/back)
        float y = Mathf.Sin(t * 0.5f) * swayAmountY; // subtle twist

        transform.localRotation = Quaternion.Euler(x, y, z);
    }
}