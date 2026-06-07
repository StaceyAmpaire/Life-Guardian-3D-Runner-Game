using System.Collections;
using UnityEngine;
using TMPro;

public class MealChoicePopupManager : MonoBehaviour
{
    public GameObject mealChoicePanel;
    public TMP_Text questionText;

    public float showDuration = 10f;

    private string[] questions =
    {
        "What did you eat for breakfast?",
        "What did you eat for lunch?",
        "What did you eat for supper?"
    };

    private float[] popupTimes =
    {
        60f,   // 1 minute
        90f,   // 1.5 minutes
        120f   // 2 minutes
    };

    void Start()
    {
        mealChoicePanel.SetActive(false);

        StartCoroutine(ShowMealPopups());
    }

    IEnumerator ShowMealPopups()
    {
        float previousTime = 0f;

        for (int i = 0; i < popupTimes.Length; i++)
        {
            yield return new WaitForSeconds(popupTimes[i] - previousTime);

            previousTime = popupTimes[i];

            ShowPopup(questions[i]);

            yield return new WaitForSecondsRealtime(showDuration);

            HidePopup();
        }
    }

    void ShowPopup(string question)
    {
        questionText.text = question;

        mealChoicePanel.SetActive(true);

        // Pause game
        Time.timeScale = 0f;
    }

    public void SelectFood(FoodItem food)
{
    if (food == null) return;

    TopBarManager topBar = FindObjectOfType<TopBarManager>();

    if (topBar != null)
    {
        topBar.AddScore(food.scorePoints);
    }

    FoodCollectionManager manager = FindObjectOfType<FoodCollectionManager>();

    if (manager != null)
    {
        manager.RecordFood(
            food.foodName,
            food.foodMessage,
            food.scorePoints,
            food.healthyFood
        );
    }

    HidePopup();
}

    void HidePopup()
    {
        mealChoicePanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;
    }
}