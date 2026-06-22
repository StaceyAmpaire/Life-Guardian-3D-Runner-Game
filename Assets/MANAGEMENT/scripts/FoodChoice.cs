using UnityEngine;

public class FoodChoice : MonoBehaviour
{
    public string foodName;
    public int points = 250;

    [TextArea(2, 4)]
    public string foodMessage;

    public RemyController remyController;
    public MealPopupManager popupManager;
    public ChoiceFeedbackPopup feedbackPopup;

    void Start()
    {
        if (remyController == null)
            remyController = FindFirstObjectByType<RemyController>();

        if (popupManager == null)
            popupManager = FindFirstObjectByType<MealPopupManager>();

        if (feedbackPopup == null)
            feedbackPopup = FindFirstObjectByType<ChoiceFeedbackPopup>();
    }

    public void SelectFood()
    {
        if (remyController == null) return;

        if (points >= 0)
{
    remyController.AddPoints(5);

    MasterInfo.dewCount += 2;
    MasterInfo.totalDewCount += 2;
    MasterInfo.SaveData();
}
else
{
    remyController.RemovePoints(5);
}

MasterInfo.Instance.UpdateLifeDisplay();
MasterInfo.Instance.UpdateDewDisplay();
MasterInfo.CheckAndUnlockLevel2();

        remyController.ShowChoiceMessage(foodMessage);

        if (feedbackPopup != null)
            feedbackPopup.ShowFeedback(points, foodMessage);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayChoiceSound(points);

        if (popupManager != null)
            popupManager.ClosePopup();
    }
}