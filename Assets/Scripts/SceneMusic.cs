using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip menuMusic; // Music for Splash, MainMenu, EnvironmentChoice, LevelSelect
    // public AudioClip runMusic;  // No longer needed here, as Run scene will manage its own music

    [Header("Music Settings")]
    [Range(0f, 1f)] public float menuMusicVolume = 0.7f;
    [Range(0.1f, 3f)] public float menuMusicPitch = 1.0f;
    // Run scene specific volume/pitch settings are now managed by the BGM object in LevelControls

    void Awake()
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
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Determine initial music based on the scene loaded first
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "SplashScene": // Your splash screen
            case "MainMenu":
            case "EnvironmentChoice": // Your environment choice scene
            case "LevelSelectScene":
                PlayMenuMusic();
                break;

            case "LoadingScene":
            case "Run": // In the Run scene, the MusicManager will stop its music
                audioSource.Stop();
                break;

            default:
                // For any other scenes not explicitly handled, stop the music
                audioSource.Stop();
                break;
        }
    }

    public void PlayMenuMusic()
    {
        // If the clip is different, set it and play. Otherwise, just update volume/pitch.
        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        audioSource.volume = menuMusicVolume;
        audioSource.pitch = menuMusicPitch;

        if (!audioSource.isPlaying && audioSource.clip != null) // Ensure it plays if it was stopped and now needs to play menu music
        {
            audioSource.Play();
        }
    }

    // PlayRunMusic() is no longer needed here as the Run scene will manage its own music
    // public void PlayRunMusic()
    // {
    //     // ... (removed logic)
    // }
}
