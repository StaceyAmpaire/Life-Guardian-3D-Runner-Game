using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseAvatarButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnChooseAvatarClicked);
    }

    private void OnChooseAvatarClicked()
    {
        Debug.Log("📱 Loading Avatar Selection Scene...");
        SceneManager.LoadScene("AvatarSelection");
    }
}
