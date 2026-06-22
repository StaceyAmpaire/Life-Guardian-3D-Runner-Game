using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;

    [Header("Music Settings")]
    [Range(0f, 1f)] public float menuMusicVolume = 0.7f;
    [Range(0.1f, 3f)] public float menuMusicPitch = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ShouldUseMenuMusic(scene.name))
        {
            PlayMenuMusic();
        }
        else
        {
            StopMenuMusic();
        }
    }

    private bool ShouldUseMenuMusic(string sceneName)
    {
        return sceneName == "SplashScene"
            || sceneName == "MainMenu"
            || sceneName == "EnvironmentChoice"
            || sceneName == "LevelSelectScene"
            || sceneName == "AvatarSelection"
            || sceneName == "AchievementsScene";
    }

    public void PlayMenuMusic()
    {
        if (audioSource == null) return;

        if (!IsMusicEnabled())
        {
            audioSource.Stop();
            return;
        }

        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
        }

        audioSource.loop = true;
        audioSource.volume = menuMusicVolume;
        audioSource.pitch = menuMusicPitch;

        if (!audioSource.isPlaying && audioSource.clip != null)
            audioSource.Play();
    }

    public void StopMenuMusic()
    {
        if (audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = null;   // VERY IMPORTANT
    }

    public void ApplyMusicSetting()
    {
        if (audioSource == null) return;

        Scene currentScene = SceneManager.GetActiveScene();

        if (!ShouldUseMenuMusic(currentScene.name))
        {
            // In scenes like LoadingScene / Run / Management,
            // this manager should stay silent.
            StopMenuMusic();
            return;
        }

        if (IsMusicEnabled())
        {
            PlayMenuMusic();
        }
        else
        {
            StopMenuMusic();
        }
    }

    private bool IsMusicEnabled()
    {
        if (AudioSettingsManager.Instance == null)
            return true;

        return AudioSettingsManager.Instance.MusicEnabled;
    }
}