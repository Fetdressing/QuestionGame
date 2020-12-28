using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField]
    private ToggleInterface effectSoundToggle;

    private void OnEnable()
    {
        effectSoundToggle.Set(AudioManager.EffectsOn, (newValue) => { AudioManager.EffectsOn = newValue; });
    }
}
