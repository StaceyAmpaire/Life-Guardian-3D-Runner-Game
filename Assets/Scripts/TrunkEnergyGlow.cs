using UnityEngine;

public class TrunkEnergyGlow : MonoBehaviour
{
    public SpriteRenderer glowLayer;
    public float pulseSpeed = 2f;
    public float minAlpha = 0.2f; // Lowered slightly so you can see it better
    public float maxAlpha = 0.7f;

    private Color baseColor = Color.white;

    void Update()
    {
        if (glowLayer == null) return;

        // Calculate the pulse
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);

        // Apply pulse to the base color provided by the StateController
        Color finalColor = baseColor;
        finalColor.a = a; 

        glowLayer.color = finalColor;
    }

    public void SetBaseColor(Color newColor)
    {
        baseColor = newColor;
    }
}
