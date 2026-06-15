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
    private int currentScore = 0;  // This accumulates across ALL meals

    // Variables for 3-choice limit
    private List<FoodChoice> selectedFoodsForCurrentMeal = new List<FoodChoice>();
    private int maxChoicesPerMeal = 3;

    // Reference to RemyController to add points
    public RemyController remyController;
    [Header("Question UI")]
    public GameObject questionObject;

    void Start()
    {
        popupPanel.SetActive(false);

        if (questionText != null)
            questionText.gameObject.SetActive(false);

        // Find RemyController if not assigned
        if (remyController == null)
            remyController = FindObjectOfType<RemyController>();

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

        popupPanel.SetActive(true);

        if (questionText != null)
        {
            questionText.gameObject.SetActive(true);
            questionText.text = question;
        }

        // Stop any existing coroutine to prevent premature closing
        StopAllCoroutines();
        // Start the auto-hide timer (but don't let it close if user is selecting)
        StartCoroutine(AutoHidePopupAfterSeconds(8f)); // Increased time
    }

    IEnumerator AutoHidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        // Only auto-close if no selections were made
        if (selectedFoodsForCurrentMeal.Count == 0)
        {
            popupPanel.SetActive(false);
            if (questionText != null)
                questionText.gameObject.SetActive(false);
        }
    }

    IEnumerator HidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        popupPanel.SetActive(false);

        if (questionText != null)
            questionText.gameObject.SetActive(false);
    }
    public void SelectFood(FoodChoice food)
        {
            Debug.Log("BUTTON CLICKED");

            if (food == null)
            {
                Debug.LogWarning("FoodChoice is missing!");
                return;
            }

            if (remyController != null)
            {
                if (food.points >= 0)
                    remyController.AddPoints(food.points);
                else
                    remyController.RemovePoints(-food.points);
            }
            else
            {
                Debug.LogError("RemyController is not assigned in MealPopupManager!");
            }

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayChoiceSound(food.points);

            Debug.Log("Selected: " + food.foodName + " points: " + food.points);

            selectedFoodsForCurrentMeal.Add(food);

            if (selectedFoodsForCurrentMeal.Count >= maxChoicesPerMeal)
            {
                ClosePopup();
            }
        }

            // Optional: Method to get current score
            public int GetCurrentScore()
            {
                return currentScore;
            }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        if (questionText != null)
            questionText.gameObject.SetActive(false);
    }
}