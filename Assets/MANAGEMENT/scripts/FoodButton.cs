using UnityEngine;

public class FoodButton : MonoBehaviour
{
    public int points;

    public RemyController remyController;
    public MealPopupManager popupManager;

    public void SelectFood()
    {
        Debug.Log("Food Button Clicked!");

        if (remyController != null)
        {
            if (points >= 0)
                remyController.AddPoints(points);
            else
                remyController.RemovePoints(-points);
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayChoiceSound(points);

        if (popupManager != null)
            popupManager.ClosePopup();
    }
}