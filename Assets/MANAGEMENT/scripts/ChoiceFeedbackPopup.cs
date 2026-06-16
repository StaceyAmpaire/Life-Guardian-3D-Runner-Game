using UnityEngine;
using TMPro;
using System.Collections;

public class ChoiceFeedbackPopup : MonoBehaviour
{
    public GameObject feedbackPanel;
    public TMP_Text feedbackText;

    void Start()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    public void ShowFeedback(int points)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackPanel.SetActive(true);

        if (points >= 0)
            feedbackText.text = "👏 Good choice! Well done.";
        else
            feedbackText.text = "⚠️ Bad choice! Try a healthier option next time.";

        StopAllCoroutines();
        StartCoroutine(HideAfterSeconds(1.5f));
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        feedbackPanel.SetActive(false);
    }
}