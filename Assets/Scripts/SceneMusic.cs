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
        if (scene.name == "LoadingScene")
        {
            audioSource.Stop();
        }

        if (scene.name == "Run")
        {
            audioSource.Stop();
        }

        if (scene.name == "LevelSelectScene")
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}