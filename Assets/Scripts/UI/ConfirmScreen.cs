using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ConfirmScreen : UIBase
{
    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private TextMeshProUGUI displayText;

    public static ConfirmScreen Create()
    {
        return (ConfirmScreen)UIUtil.Spawn(UIUtil.UIType.ConfirmScreen);
    }

    public void Set(string displayText = null, System.Action confirm = null, System.Action cancel = null, bool useCancel = true)
    {
        if (string.IsNullOrEmpty(displayText))
        {
            this.displayText.gameObject.SetActive(false);
        }
        else
        {
            this.displayText.gameObject.SetActive(true);
            this.displayText.text = displayText;
        }

        confirmButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(cancel != null || useCancel);

        confirmButton.onClick.AddListener(() => { confirm?.Invoke(); OnConfirm(); });
        cancelButton.onClick.AddListener(() => { cancel?.Invoke(); OnCancel(); });
    }

    protected virtual void OnConfirm()
    {
        DestroySelf();
    }

    protected virtual void OnCancel()
    {
        DestroySelf();
    }
}
