using UnityEngine;

public class LeafLayerSway : MonoBehaviour
{
    public float swayAmount = 2f;
    public float swaySpeed = 1f;
    public float offset;

    void Start()
    {
        offset = Random.Range(0f, 10f);
    }

    void Update()
    {
        float t = Time.time * swaySpeed + offset;

        float x = Mathf.Sin(t) * swayAmount;
        float y = Mathf.Cos(t * 0.6f) * (swayAmount * 0.3f);

        transform.localRotation = Quaternion.Euler(y, x, 0);
    }
}