using UnityEngine;

public static class SoundSettings
{
    private const string MUSIC_KEY = "MusicEnabled";
    private const string SFX_KEY = "SfxEnabled";

    public static bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
    }

    public static bool IsSfxEnabled()
    {
        return PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    public static void SetMusicEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(MUSIC_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void SetSfxEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(SFX_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void ToggleMusic()
    {
        SetMusicEnabled(!IsMusicEnabled());
    }

    public static void ToggleSfx()
    {
        SetSfxEnabled(!IsSfxEnabled());
    }
}