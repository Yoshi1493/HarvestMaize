using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    AudioSource aux;

    void Awake()
    {
        if (Instance == null) Instance = this;

        aux = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, bool allowOverlap = false)
    {
        if (clip != null)
        {
            if (!aux.isPlaying || allowOverlap)
            {
                aux.PlayOneShot(clip);
            }
        }
    }
}