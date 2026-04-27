using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BackButtonHandler : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GoBackToMainHub);
    }

    private void GoBackToMainHub()
    {
        Debug.Log("Going back to Main Hub...");
        SceneManager.LoadScene("MainMenu"); // Change to your main scene name
    }
}
