using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip goodChoiceSound;
    public AudioClip badChoiceSound;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
        }

        ApplyAudioSettings();
    }

    public void PlayChoiceSound(int points)
    {
        if (!IsSfxEnabled())
            return;

        if (sfxSource == null)
            return;

        if (points > 0 && goodChoiceSound != null)
            sfxSource.PlayOneShot(goodChoiceSound);
        else if (points < 0 && badChoiceSound != null)
            sfxSource.PlayOneShot(badChoiceSound);
    }

    public void ApplyAudioSettings()
    {
        if (musicSource != null)
        {
            if (IsMusicEnabled())
            {
                if (musicSource.clip != null && !musicSource.isPlaying)
                    musicSource.Play();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    private bool IsMusicEnabled()
    {
        if (AudioSettingsManager.Instance == null)
            return true;

        return AudioSettingsManager.Instance.MusicEnabled;
    }

    private bool IsSfxEnabled()
    {
        if (AudioSettingsManager.Instance == null)
            return true;

        return AudioSettingsManager.Instance.SfxEnabled;
    }
}