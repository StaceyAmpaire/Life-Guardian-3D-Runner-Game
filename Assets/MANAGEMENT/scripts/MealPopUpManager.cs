using UnityEngine;
using TMPro;

public class MealPopupManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text mealTitle;

    private bool breakfastShown;
    private bool lunchShown;
    private bool supperShown;

    private float timer;

    void Start()
    {
        popupPanel.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!breakfastShown && timer >= 20f)
        {
            ShowMeal("Breakfast");
            breakfastShown = true;
        }

        if (!lunchShown && timer >= 50f)
        {
            ShowMeal("Lunch");
            lunchShown = true;
        }

        if (!supperShown && timer >= 80f)
        {
            ShowMeal("Supper");
            supperShown = true;
        }
    }

    void ShowMeal(string mealName)
    {
        popupPanel.SetActive(true);
        mealTitle.text = mealName;

        Time.timeScale = 0f;
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}