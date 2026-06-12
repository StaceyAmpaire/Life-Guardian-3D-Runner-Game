using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public float amount = 5f;
    public float speed = 2f;

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * amount;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}