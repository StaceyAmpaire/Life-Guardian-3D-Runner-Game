using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FoodCollectionManager : MonoBehaviour
{
    public GameObject finalPanel;
    public TMP_Text finalTitleText;
    public TMP_Text finalMessageText;

    private List<string> collectedFoods = new List<string>();

    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);
        else
            Debug.LogError("Final Panel is NOT assigned in FoodCollectionManager.");
    }

    public void RecordFood(string foodName, string foodMessage, int points, bool healthyFood)
        {
            string type = healthyFood ? "Healthy choice" : "Less healthy choice";

            string record =
                "Food: " + foodName + "\n" +
                "Type: " + type + "\n" +
                "Message: " + foodMessage;

            collectedFoods.Add(record);
        }

    public void ShowFinalFoodReport()
    {
        Debug.Log("ShowFinalFoodReport called");

        if (finalPanel == null)
        {
            Debug.LogError("Final Panel is missing. Drag FinalFoodChoicePanel into FoodCollectionManager.");
            return;
        }

        finalPanel.SetActive(true);

        if (finalTitleText != null)
            finalTitleText.text = "Nutritional Report of Collected Foods";

        if (finalMessageText != null)
        {
            if (collectedFoods.Count == 0)
            {
                finalMessageText.text =
                    "Remy did not collect any food this round.\n\nTry again and choose foods that support steady energy.";
            }
            else
            {
                finalMessageText.text =
                    "Here are the foods You collected:\n\n" +
                    string.Join("\n\n", collectedFoods);
            }
        }

        Time.timeScale = 0f;
    }
}