using System;
using UnityEngine;

[Serializable]
public class SoundEffect
{
    public AudioClip clip;

    [HideInInspector] public AudioSource source;
    [HideInInspector] public string name;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] SoundEffect[] soundEffects;
    [SerializeField] Transform soundEffectParent;

    void Awake()
    {
        if (Instance == null) Instance = this;

        //init.sound effects
        foreach (var sound in soundEffects)
        {
            sound.source = soundEffectParent.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.name = sound.source.clip.name;
        }
    }

    public void PlaySound(string name, bool allowOverlap = false)
    {
        SoundEffect sound = Array.Find(soundEffects, s => s.name == name);
        if (sound == null) return;

        if (!sound.source.isPlaying || allowOverlap)
        {
            sound.source.Play();
        }
    }
}