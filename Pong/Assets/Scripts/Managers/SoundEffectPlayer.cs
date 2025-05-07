using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer instance;

    [SerializeField] private AudioSource soundEffectObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // This will be used to play all sounds that need to persist between scenes
    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // First spawn in our object
        AudioSource audioSource = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip to it
        audioSource.clip = audioClip;

        // Assign the volume
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Make the sound DDOL
        DontDestroyOnLoad(audioSource.gameObject);

        // Get the length of it
        float clipLength = audioSource.clip.length;

        // Destroy the clip when done
        Destroy(audioSource.gameObject, clipLength);
    }

    // This will be for all sounds that should not persist between scenes
    public void PlaySoundClipHere(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // First spawn in our object
        AudioSource audioSource = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip to it
        audioSource.clip = audioClip;

        // Assign the volume
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Get the length of it
        float clipLength = audioSource.clip.length;

        // Destroy the clip when done
        Destroy(audioSource.gameObject, clipLength);
    }
}
