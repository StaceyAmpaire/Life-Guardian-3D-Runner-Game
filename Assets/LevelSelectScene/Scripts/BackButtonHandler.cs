using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BackButtonHandler : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GoBackToEnvironmentChoice);

    }

    private void GoBackToEnvironmentChoice()
{
    Debug.Log("Going back to Environment Choice...");
    SceneManager.LoadScene("EnvironmentChoice");
}

}
