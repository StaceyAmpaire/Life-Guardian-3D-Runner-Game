using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectSwipe : MonoBehaviour
{
    [SerializeField] private GameObject maleAvatar;
    [SerializeField] private GameObject femaleAvatar;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Image dotMale;
[SerializeField] private Image dotFemale;

    private int currentAvatar = 0;

    private void Start()
    {
        leftButton.onClick.AddListener(PreviousAvatar);
        rightButton.onClick.AddListener(NextAvatar);
        selectButton.onClick.AddListener(SelectAvatar);

        currentAvatar =
            PlayerPrefs.GetInt("SelectedAvatar", 0);

        UpdateDisplay();
        UpdateDots();
    }

    private void NextAvatar()
{
    currentAvatar++;

    if (currentAvatar > 1)
        currentAvatar = 0;

    UpdateDisplay();
    UpdateDots();
}

    private void PreviousAvatar()
{
    currentAvatar--;

    if (currentAvatar < 0)
        currentAvatar = 1;

    UpdateDisplay();
    UpdateDots();
}

    private void SelectAvatar()
    {
        PlayerPrefs.SetInt(
            "SelectedAvatar",
            currentAvatar
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Avatar Selected: " + currentAvatar
        );
    }

    private void UpdateDisplay()
    {
        maleAvatar.SetActive(
            currentAvatar == 0
        );

        femaleAvatar.SetActive(
            currentAvatar == 1
        );
    }
    private void UpdateDots()
{
    if (currentAvatar == 0)
    {
        dotMale.color = new Color(1f, 0.78f, 0f);
        dotFemale.color = new Color(0.39f, 0.39f, 0.39f);
    }
    else
    {
        dotMale.color = new Color(0.39f, 0.39f, 0.39f);
        dotFemale.color = new Color(1f, 0.78f, 0f);
    }
}
}