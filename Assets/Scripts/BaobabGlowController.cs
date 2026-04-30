using UnityEngine;

public class BaobabGlowController : MonoBehaviour
{
    public SpriteRenderer glowLayer;

    public float pulseSpeed = 2f;
    public float minBrightness = 0.85f;
    public float maxBrightness = 1.2f;

    private Color baseColor;

    void Start()
    {
        baseColor = glowLayer.color;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float glow = Mathf.Lerp(minBrightness, maxBrightness, t);

        Color c = baseColor;
        c.r *= glow;
        c.g *= glow;
        c.b *= glow;

        glowLayer.color = c;
    }

    public void SetBaseColor(Color newColor)
    {
        baseColor = newColor;
    }
}