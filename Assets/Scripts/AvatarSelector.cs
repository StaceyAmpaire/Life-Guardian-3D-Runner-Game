using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    public GameObject malePreview;
    public GameObject femalePreview;

    private int currentAvatar = 0;

    private void Start()
    {
        currentAvatar =
            PlayerPrefs.GetInt("SelectedAvatar", 0);

        UpdateDisplay();
    }

    public void NextAvatar()
    {
        currentAvatar++;

        if (currentAvatar > 1)
            currentAvatar = 0;

        UpdateDisplay();
    }

    public void PreviousAvatar()
    {
        currentAvatar--;

        if (currentAvatar < 0)
            currentAvatar = 1;

        UpdateDisplay();
    }

    public void SelectAvatar()
    {
        PlayerPrefs.SetInt(
            "SelectedAvatar",
            currentAvatar
        );

        PlayerPrefs.Save();
    }

    void UpdateDisplay()
    {
        malePreview.SetActive(
            currentAvatar == 0
        );

        femalePreview.SetActive(
            currentAvatar == 1
        );
    }
}