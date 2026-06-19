using UnityEngine;

public class MgtAvatarLoader : MonoBehaviour
{
    public GameObject maleAvatar;
    public GameObject femaleAvatar;

    public RemyController remyController;

    void Start()
    {
        int selectedAvatar = PlayerPrefs.GetInt("SelectedAvatar", 0);

        maleAvatar.SetActive(selectedAvatar == 0);
        femaleAvatar.SetActive(selectedAvatar == 1);

        GameObject activeAvatar =
            selectedAvatar == 0 ? maleAvatar : femaleAvatar;

        // 🔥 IMPORTANT: assign Animator from active avatar
        Animator anim = activeAvatar.GetComponentInChildren<Animator>();

        if (remyController != null)
        {
            remyController.animator = anim;
        }
        else
        {
            Debug.LogError("RemyController not assigned in MgtAvatarLoader");
        }
    }
}