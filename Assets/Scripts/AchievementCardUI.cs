using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementCardUI : MonoBehaviour
{
    [Header("Basic")]
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Progress")]
    public TMP_Text progressText;
    public Image circleFill;
    public TMP_Text percentText;
    public TMP_Text rewardText;

    [Header("Groups")]
    public GameObject progressGroup;
    public GameObject claimGroup;
    public GameObject completedGroup;

    [Header("Claim")]
    public Button claimButton;

    public void ShowProgress()
    {
        progressGroup.SetActive(true);
        claimGroup.SetActive(false);
        completedGroup.SetActive(false);
    }

    public void ShowClaim()
    {
        progressGroup.SetActive(false);
        claimGroup.SetActive(true);
        completedGroup.SetActive(false);
    }

    public void ShowCompleted()
    {
        progressGroup.SetActive(false);
        claimGroup.SetActive(false);
        completedGroup.SetActive(true);
    }

    public void SetTexts(string title, string description, int reward)
    {
        if (titleText != null)
            titleText.text = title;

        if (descriptionText != null)
            descriptionText.text = description;

        if (rewardText != null)
            rewardText.text = reward.ToString();
    }

    public void SetProgress(int current, int target)
    {
        float value = (float)current / target;

        if (circleFill != null)
            circleFill.fillAmount = value;

        if (progressText != null)
            progressText.text = current + "/" + target;

        if (percentText != null)
            percentText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetProgressAnimated(int current, int target)
{
    float value = (float)current / target;

    if (circleFill != null)
        circleFill.fillAmount = value;

    if (progressText != null)
        progressText.text = current + "/" + target;

    if (percentText != null)
        percentText.text = Mathf.RoundToInt(value * 100) + "%";
}
}