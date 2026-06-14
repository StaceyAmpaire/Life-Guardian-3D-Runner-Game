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

    void Start()
    {
        popupPanel.SetActive(false);

        // Find RemyController if not assigned
        if (remyController == null)
            remyController = FindObjectOfType<RemyController>();

        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!breakfastShown && timer >= 30f)
        {
            ShowPopup("What did you eat for Breakfast? (Max 3 choices)");
            breakfastShown = true;
        }

        if (!snackShown && timer >= 50f)
        {
            ShowPopup("What did you snack on? (Max 3 choices)");
            snackShown = true;
        }

        if (!lunchShown && timer >= 80f)
        {
            ShowPopup("What did you eat for Lunch? (Max 3 choices)");
            lunchShown = true;
        }

        if (!supperShown && timer >= 100f)
        {
            ShowPopup("What did you eat for Supper? (Max 3 choices)");
            supperShown = true;
        }
    }

    void ShowPopup(string question)
    {
        selectedFoodsForCurrentMeal.Clear();  // Clear only for this meal
        popupPanel.SetActive(true);
        questionText.text = question;

        StopAllCoroutines();
        StartCoroutine(HidePopupAfterSeconds(5f));  // Changed from 3f to 5f
    }

    IEnumerator HidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        popupPanel.SetActive(false);
    }

    public void SelectFood(FoodChoice food)
    {
        // Check if max choices reached
        if (selectedFoodsForCurrentMeal.Count >= maxChoicesPerMeal)
        {
            Debug.Log("Maximum " + maxChoicesPerMeal + " food choices allowed for this meal!");
            return;
        }

        // Check if this food was already selected
        if (selectedFoodsForCurrentMeal.Contains(food))
        {
            Debug.Log("You already selected " + food.foodName);
            return;
        }

        // Add food to current meal selection
        selectedFoodsForCurrentMeal.Add(food);
        
        // ADD POINTS to running total
        currentScore += food.points;

        // Update UI
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;

        // Also update RemyController score if it exists
        if (remyController != null)
        {
            remyController.AddPoints(food.points);
        }

        Debug.Log("Selected: " + food.foodName + " (+" + food.points + " points) | Total: " + currentScore + 
                  " | This meal: " + selectedFoodsForCurrentMeal.Count + "/" + maxChoicesPerMeal);

        // Auto-close after 3 selections
        if (selectedFoodsForCurrentMeal.Count >= maxChoicesPerMeal)
        {
            popupPanel.SetActive(false);
        }
    }

    // Optional: Method to get current score
    public int GetCurrentScore()
    {
        return currentScore;
    }
}