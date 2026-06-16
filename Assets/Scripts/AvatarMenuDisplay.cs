using UnityEngine;

public class AvatarMenuDisplay : MonoBehaviour
{
    public GameObject maleAvatar;
    public GameObject femaleAvatar;

    void Start()
    {
        int selectedAvatar =
            PlayerPrefs.GetInt("SelectedAvatar", 0);

        maleAvatar.SetActive(
            selectedAvatar == 0
        );

        femaleAvatar.SetActive(
            selectedAvatar == 1
        );
    }
}