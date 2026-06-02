using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLifeManager : MonoBehaviour
{
    public static PlayerLifeManager Instance { get; private set; }

    [Header("UI References")]
    public Slider lifeSlider; 
    public TMP_Text lifeValueText; 
    public Image lifeFillImage; 

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UpdateLifeUI(float currentLife)
    {
        if (lifeSlider != null) lifeSlider.value = currentLife;
        if (lifeValueText != null) lifeValueText.text = currentLife.ToString("F0");

        if (lifeFillImage != null)
        {
            // Color logic: Red at 45 or below, Green above 50
            if (currentLife <= 45f) lifeFillImage.color = Color.red;
            else if (currentLife < 50f) lifeFillImage.color = Color.yellow;
            else lifeFillImage.color = Color.green;
        }
    }
}
