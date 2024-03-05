using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGameOver : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip backgroundGameOverSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PlayBackgroundGameOverSound();
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, 1f);
        }
    }

    private void PlayBackgroundGameOverSound()
    {
        if (backgroundGameOverSound != null && audioSource != null)
        {
            audioSource.clip = backgroundGameOverSound;
            audioSource.PlayOneShot(backgroundGameOverSound, 0.5f);
        }
    }
}
