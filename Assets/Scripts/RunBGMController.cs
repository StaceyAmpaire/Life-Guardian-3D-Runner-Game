using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RunBGMController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ApplyMusicSetting();
    }

    public void ApplyMusicSetting()
    {
        bool musicEnabled = true;

        if (AudioSettingsManager.Instance != null)
            musicEnabled = AudioSettingsManager.Instance.MusicEnabled;

        if (musicEnabled)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}