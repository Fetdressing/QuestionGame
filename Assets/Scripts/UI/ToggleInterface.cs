using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInterface : UIBase
{
    [SerializeField]
    private UnityButtonInterface toggleButton;

    [SerializeField]
    private RectTransform[] onGraphics;

    [SerializeField]
    private RectTransform[] offGraphics;

    private bool isOn;
    private System.Action<bool> onChanged;

    private void Awake()
    {
        toggleButton.onClick.AddListener(delegate { this.IsOn = !this.IsOn; });
    }

    public void Set(bool toggled, System.Action<bool> onChanged = null)
    {
        this.IsOn = toggled;
        this.onChanged = onChanged;
    }

    public bool IsOn
    {
        get
        {
            return this.isOn;
        }

        private set
        {
            bool oldVal = this.isOn;
            this.isOn = value;

            for (int i = 0; i < onGraphics.Length; i++)
            {
                onGraphics[i].gameObject.SetActive(this.isOn);
            }

            for (int i = 0; i < offGraphics.Length; i++)
            {
                offGraphics[i].gameObject.SetActive(!this.isOn);
            }

            if (oldVal != value)
            {
                this.onChanged?.Invoke(value);
            }
        }
    }
}
