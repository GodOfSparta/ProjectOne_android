using UnityEngine;

public class SoundManagerMainMenu : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;

    public float soundVolume = 1.0f;
    public float musicVolume = 0.5f;  // Пример значения громкости для музыки

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlayBackgroundMusic();
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, soundVolume);
        }
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
