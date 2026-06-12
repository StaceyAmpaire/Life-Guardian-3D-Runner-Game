using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BacktoMain : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GoBackToMainMenu);
    }

    private void GoBackToMainMenu()
    {
        Debug.Log("Going back to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
