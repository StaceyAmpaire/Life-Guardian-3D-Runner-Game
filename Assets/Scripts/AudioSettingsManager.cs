using UnityEngine;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    private const string MUSIC_KEY = "MusicEnabled";
    private const string SFX_KEY = "SfxEnabled";

    public bool MusicEnabled { get; private set; } = true;
    public bool SfxEnabled { get; private set; } = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MusicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        SfxEnabled = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    public void SetMusicEnabled(bool enabled)
    {
        MusicEnabled = enabled;
        PlayerPrefs.SetInt(MUSIC_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSfxEnabled(bool enabled)
    {
        SfxEnabled = enabled;
        PlayerPrefs.SetInt(SFX_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleMusic()
    {
        SetMusicEnabled(!MusicEnabled);
    }

    public void ToggleSfx()
    {
        SetSfxEnabled(!SfxEnabled);
    }
}