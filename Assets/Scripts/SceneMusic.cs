using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicManager : MonoBehaviour
{
    public static SceneMusicManager instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop menu music before Run scene
        if (scene.name == "Run")
        {
            audioSource.Stop();
            Destroy(gameObject);
        }

        // Stop music in Loading scene (optional but clean)
        if (scene.name == "LoadingScene")
        {
            audioSource.Stop();
        }
    }
}