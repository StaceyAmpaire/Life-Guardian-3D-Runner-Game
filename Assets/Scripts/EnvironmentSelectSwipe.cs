using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnvironmentSelectSwipe : MonoBehaviour
{
    [SerializeField] private RectTransform desertCard;
    [SerializeField] private RectTransform cityCard;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Image dotDesert;
    [SerializeField] private Image dotCity;
    
    private int currentCard = 0; // 0 = Desert, 1 = City
    private bool isAnimating = false;
    private float swipeSpeed = 0.6f;
    
    private void Start()
    {
        leftButton.onClick.AddListener(SwipeLeft);
        rightButton.onClick.AddListener(SwipeRight);
        playButton.onClick.AddListener(Play);
        
        UpdateDots();
    }
    
    private void SwipeLeft()
    {
        if (isAnimating) return;
        currentCard = (currentCard - 1 + 2) % 2;
        StartCoroutine(AnimateSwipe());
    }
    
    private void SwipeRight()
    {
        if (isAnimating) return;
        currentCard = (currentCard + 1) % 2;
        StartCoroutine(AnimateSwipe());
    }
    
    private IEnumerator AnimateSwipe()
    {
        isAnimating = true;
        float elapsed = 0f;
        
        Vector3 desertStart = desertCard.localPosition;
        Vector3 cityStart = cityCard.localPosition;
        
       Vector3 desertEnd = currentCard == 0 ? new Vector3(0, 5.6706f, 0) : new Vector3(-1920, 5.6706f, 0);
Vector3 cityEnd = currentCard == 1 ? new Vector3(0, 5.6706f, 0) : new Vector3(1920, 5.6706f, 0);

        while (elapsed < swipeSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swipeSpeed;
            t = t * t * (3f - 2f * t);
            
            desertCard.localPosition = Vector3.Lerp(desertStart, desertEnd, t);
            cityCard.localPosition = Vector3.Lerp(cityStart, cityEnd, t);
            
            yield return null;
        }
        
        desertCard.localPosition = desertEnd;
        cityCard.localPosition = cityEnd;
        
        UpdateDots();
        isAnimating = false;
    }
    
    private void UpdateDots()
    {
        if (currentCard == 0)
        {
            dotDesert.color = new Color(1f, 0.78f, 0f);
            dotCity.color = new Color(0.39f, 0.39f, 0.39f);
        }
        else
        {
            dotDesert.color = new Color(0.39f, 0.39f, 0.39f);
            dotCity.color = new Color(1f, 0.78f, 0f);
        }
    }
    
    private void Play()
    {
        string sceneName = currentCard == 0 ? "LevelSelectScene" : "CitySublevelSelect";

        SceneManager.LoadScene(sceneName);
    }
}