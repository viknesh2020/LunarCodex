using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    public enum SoundType
    {
        Tap,
        CardMatched,
        CardMismatched,
        LevelComplete,
        LevelFailed
    }

    [System.Serializable]
    public class SoundEntry
    {
        public SoundType soundType;
        public AudioClip audioClip;
    }

    [Header("Sound Effect List")]
    public List<SoundEntry> soundEntries; 

    private AudioSource sfxSource;
    private Dictionary<SoundType, AudioClip> soundDictionary;

    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
        soundDictionary = new Dictionary<SoundType, AudioClip>();

        foreach (SoundEntry entry in soundEntries)
        {
            if (!soundDictionary.ContainsKey(entry.soundType))
            {
                soundDictionary.Add(entry.soundType, entry.audioClip);
            }
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundDictionary.TryGetValue(soundType, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}