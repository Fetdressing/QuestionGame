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
    private AudioSource effectSource;

    public enum EffectType
    {
        Click1
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

    public static void Play(EffectType effectType)
    {
        Clip clip;
        if (Instance.audioDict.TryGetValue(effectType, out clip))
        {
            Instance.effectSource.pitch = 1 + Random.Range(-clip.pitchRange, clip.pitchRange);
            Instance.effectSource.PlayOneShot(clip.aClip, clip.volume);
        }
    }

    private void Awake()
    {
        EffectsOn = EffectsOn; // Trigger sources.
    }

    private void OnEnable()
    {
        EffectsOn = EffectsOn; // Trigger sources.
    }

    [System.Serializable]
    private class AudioDict : SerializableDictionaryBase<EffectType, Clip>
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
