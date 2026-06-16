using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MealPopupManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject popupPanel;
    public TMP_Text questionText;
    public TMP_Text scoreText;

    private bool breakfastShown;
    private bool snackShown;
    private bool lunchShown;
    private bool supperShown;

    private float timer;
    private int currentScore = 0;

    private List<FoodChoice> selectedFoodsForCurrentMeal = new List<FoodChoice>();
    private int maxChoicesPerMeal = 3;

    public RemyController remyController;
    public TopBarPanel topBarPanel;
  

    [Header("Question UI")]
    public GameObject questionObject;
    public ChoiceFeedbackPopup feedbackPopup;

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (questionText != null)
            questionText.gameObject.SetActive(false);

        if (remyController == null)
            remyController = FindFirstObjectByType<RemyController>();

        if (topBarPanel == null)
            topBarPanel = FindFirstObjectByType<TopBarPanel>();

        if (feedbackPopup == null)
            feedbackPopup = FindFirstObjectByType<ChoiceFeedbackPopup>();

        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (!breakfastShown && timer >= 20f)
        {
            ShowPopup("What did you eat for Breakfast? (Max 3 choices)");
            breakfastShown = true;
        }

        if (!snackShown && timer >= 40f)
        {
            ShowPopup("What did you snack on? (Max 3 choices)");
            snackShown = true;
        }

        if (!lunchShown && timer >= 60f)
        {
            ShowPopup("What did you eat for Lunch? (Max 3 choices)");
            lunchShown = true;
        }

        if (!supperShown && timer >= 80f)
        {
            ShowPopup("What did you eat for Supper? (Max 3 choices)");
            supperShown = true;
        }
    }

    void ShowPopup(string question)
    {
        selectedFoodsForCurrentMeal.Clear();

        if (popupPanel != null)
            popupPanel.SetActive(true);

        if (questionText != null)
        {
            questionText.gameObject.SetActive(true);
            questionText.text = question;
        }

        StopAllCoroutines();
        StartCoroutine(AutoHidePopupAfterSeconds(3f));
    }

    IEnumerator AutoHidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ClosePopup();
    }

    public void SelectFood(FoodChoice food)
    {
        Debug.Log("BUTTON CLICKED - Food: " + (food != null ? food.foodName : "NULL"));

        if (food == null)
        {
            Debug.LogWarning("FoodChoice is missing!");
            return;
        }

        if (selectedFoodsForCurrentMeal.Contains(food))
        {
            Debug.Log("Already selected: " + food.foodName);
            return;
        }

        if (selectedFoodsForCurrentMeal.Count >= maxChoicesPerMeal)
        {
            Debug.Log("Maximum choices reached.");
            return;
        }

        selectedFoodsForCurrentMeal.Add(food);

        if (remyController != null)
        {
            if (food.points >= 0)
                remyController.AddPoints(food.points);
            else
                remyController.RemovePoints(-food.points);

            if (topBarPanel != null)
            {
                topBarPanel.UpdateScore(remyController.CurrentScore);
                topBarPanel.UpdateHealth(remyController.CurrentScore, remyController.maxScore);
            }
        }
        else
        {
            Debug.LogError("RemyController is not assigned in MealPopupManager!");
        }

    
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayChoiceSound(food.points);

        Debug.Log("Selected: " + food.foodName + " points: " + food.points);
    }

    public int GetCurrentScore()
    {
        if (remyController != null)
            return remyController.CurrentScore;

        return currentScore;
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (questionText != null)
            questionText.gameObject.SetActive(false);
    }
}