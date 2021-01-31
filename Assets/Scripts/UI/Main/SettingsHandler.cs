using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField]
    private ToggleInterface effectSoundToggle;

    [SerializeField]
    private ToggleInterface musicToggle;

    private void OnEnable()
    {
        effectSoundToggle.Set(AudioManager.EffectsOn, (newValue) => { AudioManager.EffectsOn = newValue; });
        musicToggle.Set(AudioManager.MusicOn, (newValue) => { AudioManager.MusicOn = newValue; });
    }
}
