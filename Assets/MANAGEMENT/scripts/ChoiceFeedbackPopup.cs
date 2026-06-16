using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChoiceFeedbackPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject feedbackPanel;
    public TMP_Text feedbackText;
    public Image feedbackIcon;
    public Image background;

    [Header("Icons")]
    public Sprite goodTickImage;
    public Sprite badMarkImage;

   

    private void Start()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    public void ShowFeedback(int points, string message)
    {
        if (feedbackPanel == null || feedbackText == null)
            return;

        feedbackPanel.SetActive(true);

        // Set message
        feedbackText.text = message;

        if (points >= 0)
        {
            // Good choice
            

            if (feedbackIcon != null)
                feedbackIcon.sprite = goodTickImage;
        }
        else
        {
            // Bad choice
        

            if (feedbackIcon != null)
                feedbackIcon.sprite = badMarkImage;
        }

        if (feedbackIcon != null)
            feedbackIcon.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(HideAfterSeconds(1.5f));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }
}