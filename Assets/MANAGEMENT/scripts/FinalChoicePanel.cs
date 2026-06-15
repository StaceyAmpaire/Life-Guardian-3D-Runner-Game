using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FinalChoicePanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject finalPanel;
    public TMP_Text choicesText;

    private List<string> allChoices = new List<string>();

    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);
    }

    public void AddChoice(string choiceName, string message, int points)
    {
        string sign = points >= 0 ? "+" : "";
        string entry = choiceName + " (" + sign + points + " points)\n" + message;

        allChoices.Add(entry);
    }

    public void ShowFinalChoices()
    {
        if (finalPanel != null)
            finalPanel.SetActive(true);

        if (choicesText != null)
        {
            if (allChoices.Count == 0)
            {
                choicesText.text = "No choices were recorded.";
                return;
            }

            choicesText.text = "Your Choices and Lessons:\n\n";

            for (int i = 0; i < allChoices.Count; i++)
            {
                choicesText.text += (i + 1) + ". " + allChoices[i] + "\n\n";
            }
        }
    }
}