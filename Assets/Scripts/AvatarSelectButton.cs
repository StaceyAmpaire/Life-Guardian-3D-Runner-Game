using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class AvatarSelectButton : MonoBehaviour
{
    [SerializeField] private Button selectButton; // Drag the SELECT button here
    
    private void Start()
    {
        selectButton.onClick.AddListener(OnSelectClicked);
    }
    
    private void OnSelectClicked()
    {
        Debug.Log("✅ Avatar Selected! Loading Main Menu...");
        
        // The AvatarSelectSwipe script already saved the selection
        // Now just load the Main Menu
        SceneManager.LoadScene("MainMenu");
    }
}
