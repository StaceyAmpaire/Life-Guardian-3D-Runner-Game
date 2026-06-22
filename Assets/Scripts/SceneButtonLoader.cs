using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    public void LoadAchievements()
    {
        SceneManager.LoadScene("AchievementsScene");
    }
}