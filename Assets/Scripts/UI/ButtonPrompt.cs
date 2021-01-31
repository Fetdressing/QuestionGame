using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityButtonInterface))]
public class ButtonPrompt : MonoBehaviour
{
    [SerializeField]
    private UnityButtonInterface button;

    [SerializeField]
    private string prompt;

    private void Awake()
    {
        button?.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                ConfirmScreen.Create().Set(prompt, useCancel: false, confirmText: "Gotcha!");
            }
        });
    }
}
