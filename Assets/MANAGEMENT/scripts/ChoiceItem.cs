using UnityEngine;
using TMPro;

public class ChoiceItem : MonoBehaviour
{
    public string choiceName;

    [TextArea]
    public string message;

    public int points = 250;

    public TMP_Text nameLabel;
    public ChoiceFeedbackPopup feedbackPopup;

    void Start()
    {
        if (nameLabel != null)
        {
            nameLabel.text = choiceName;
            nameLabel.transform.localPosition = new Vector3(0f, 2f, 0f);
        }

        if (feedbackPopup == null)
            feedbackPopup = FindFirstObjectByType<ChoiceFeedbackPopup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        RemyController remy = other.GetComponent<RemyController>();
        if (remy == null) return;

        if (points >= 0)
{
    remy.AddPoints(5);

    MasterInfo.dewCount += 2;
    MasterInfo.totalDewCount += 2;
}
else
{
    remy.RemovePoints(5);
}

MasterInfo.Instance.UpdateLifeDisplay();
MasterInfo.Instance.UpdateDewDisplay();
MasterInfo.CheckAndUnlockLevel2();

        remy.ShowChoiceMessage(message);

        if (feedbackPopup != null)
            feedbackPopup.ShowFeedback(points, message);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayChoiceSound(points);

        Destroy(gameObject);
    }
}