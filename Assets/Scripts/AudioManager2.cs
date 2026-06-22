using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    public static AudioManager2 Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Level Sounds")]
    public AudioClip levelUnlockedClip;

    [Header("Food Sounds")]
    public AudioClip wonderfulClip;
    public AudioClip greatChoiceClip;
    public AudioClip nourishingClip;

    [Header("Game Sounds")]
    public AudioClip gameOverClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayLevelUnlockedSound()
    {
        PlaySound(levelUnlockedClip);
    }

    public void PlayWonderful()
    {
        PlaySound(wonderfulClip);
    }

    public void PlayGreatChoice()
    {
        PlaySound(greatChoiceClip);
    }

    public void PlayNourishing()
    {
        PlaySound(nourishingClip);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (!IsSfxEnabled())
            return;

        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private bool IsSfxEnabled()
    {
        if (AudioSettingsManager.Instance == null)
            return true;

        return AudioSettingsManager.Instance.SfxEnabled;
    }
}