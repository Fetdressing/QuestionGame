using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RotaryHeart.Lib.SerializableDictionary;
using RotaryHeart;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioDict audioDict;

    [SerializeField]
    private MusicDict musicDict;

    [SerializeField]
    private AudioSource effectSource;

    [SerializeField]
    private AudioSource musicSource;

    public enum EffectType
    {
        Click1
    }

    public enum MusicType
    {
        Chill1
    }

    private static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }

            return instance;
        }
    }

    public static bool EffectsOn
    {
        get
        {
            int intValue = PlayerPrefs.GetInt("effectsOn", 1);
            bool value = intValue == 1 ? true : false;
            return value;
        }

        set
        {
            int setValue = value ? 1 : 0;
            PlayerPrefs.SetInt("effectsOn", setValue);
            Instance.effectSource.enabled = value;
        }
    }

    public static bool MusicOn
    {
        get
        {
            int intValue = PlayerPrefs.GetInt("musicOn", 1);
            bool value = intValue == 1 ? true : false;
            return value;
        }

        set
        {
            int setValue = value ? 1 : 0;
            PlayerPrefs.SetInt("musicOn", setValue);
            Instance.musicSource.enabled = value;

            if (!Instance.musicSource.isPlaying)
            {
                Instance.musicSource.Play();
            }
        }
    }

    public static void Play(EffectType effectType)
    {
        Clip clip;
        if (Instance.audioDict.TryGetValue(effectType, out clip))
        {
            Instance.effectSource.pitch = 1 + Random.Range(-clip.pitchRange, clip.pitchRange);
            Instance.effectSource.PlayOneShot(clip.aClip, clip.volume);
        }
    }

    public static void Play(MusicType musicType)
    {
        Clip clip;
        if (Instance.musicDict.TryGetValue(musicType, out clip))
        {
            if (Instance.musicSource.clip != null && Instance.musicSource.clip.name == clip.aClip.name && instance.musicSource.isPlaying)
            {
                return;
            }

            Instance.musicSource.Stop();
            Instance.musicSource.clip = clip.aClip;
            Instance.musicSource.volume = clip.volume;
        }
    }

    private void Awake()
    {
        Play(MusicType.Chill1);
        EffectsOn = EffectsOn; // Trigger sources.
        MusicOn = MusicOn;
    }

    private void OnEnable()
    {
        Play(MusicType.Chill1);
        EffectsOn = EffectsOn; // Trigger sources.
        MusicOn = MusicOn;
    }

    [System.Serializable]
    private class AudioDict : SerializableDictionaryBase<EffectType, Clip>
    {
    }

    [System.Serializable]
    private class MusicDict : SerializableDictionaryBase<MusicType, Clip>
    {
    }

    [System.Serializable]
    private class Clip
    {
        [SerializeField]
        public AudioClip aClip;

        [SerializeField]
        [Range(0, 1)]
        public float volume = 1f;

        [SerializeField]
        [Range(0, 1)]
        public float pitchRange = 0.02f;
    }
}
