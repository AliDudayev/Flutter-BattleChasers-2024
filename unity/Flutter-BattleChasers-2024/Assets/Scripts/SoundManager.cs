using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] soundClips; // Array of AudioClips

    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlaySound(string soundName)
    {
        // Find the AudioClip by its name
        AudioClip clip = System.Array.Find(soundClips, sound => sound.name == soundName);

        if (clip != null)
        {
            //audioSource.clip = clip;
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in the SoundManager.");
        }
    }

    public void PlaySoundRandomPitch(string soundName)
    {
        // Find the AudioClip by its name
        AudioClip clip = System.Array.Find(soundClips, sound => sound.name == soundName);

        if (clip != null)
        {
            //audioSource.clip = clip;

            // Set a random pitch within the range
            audioSource.pitch = Random.Range(0.8f, 1.2f); // Adjust range as needed (e.g., 0.9 to 1.1)

            audioSource.PlayOneShot(clip);
        }
        else  
        {
            Debug.LogWarning($"Sound '{soundName}' not found in the SoundManager.");
        }
    }

}
