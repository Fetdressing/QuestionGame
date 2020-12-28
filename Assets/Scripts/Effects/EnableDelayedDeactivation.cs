using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDelayedDeactivation : UIBase
{
    [SerializeField]
    private float delayedDisable = 3f;

    private void OnEnable()
    {
        UIUtil.InvokeDelayed(() => { this.gameObject.SetActive(false); }, delayedDisable);
    }
}
