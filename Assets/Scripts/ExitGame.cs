using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public void ExitApplication()
    {
        // This will only work in a built application (Windows, Android, etc.)
        // It will not work in the Unity Editor.
        Application.Quit();

        // If you want to stop playing in the Unity Editor when testing,
        // you can use the following line. This line should be wrapped
        // in an #if UNITY_EDITOR block to prevent it from being included
        // in actual builds, as it's an Editor-only function.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Debug.Log("Application Quit!");
    }
}
