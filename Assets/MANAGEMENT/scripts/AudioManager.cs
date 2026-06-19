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
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayChoiceSound(int points)
    {
        if (points > 0)
            sfxSource.PlayOneShot(goodChoiceSound);
        else if (points < 0)
            sfxSource.PlayOneShot(badChoiceSound);
    }
}