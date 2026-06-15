using UnityEngine;

public class FoodButton : MonoBehaviour
{
    public int points;

    public RemyController remyController;
    public MealPopupManager popupManager;

    public void SelectFood()
    {
        remyController.AddPoints(points);

        AudioManager.Instance.PlayChoiceSound(points);

        popupManager.ClosePopup();
    }
}