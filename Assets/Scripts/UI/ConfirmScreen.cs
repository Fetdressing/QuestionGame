using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ConfirmScreen : UIBase
{
    [SerializeField]
    protected UnityButtonInterface confirmButton;

    [SerializeField]
    protected UnityButtonInterface cancelButton;

    [SerializeField]
    protected TextMeshProUGUI displayText;

    public static ConfirmScreen Create(UIUtil.UIType type = UIUtil.UIType.ConfirmScreen)
    {
        return (ConfirmScreen)UIUtil.Spawn(type);
    }

    public void Set(string displayText = null, System.Action confirm = null, System.Action cancel = null, bool useCancel = true, string confirmText = "Yes", string cancelText = "Cancel")
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

        confirmButton.Text = confirmText;
        cancelButton.Text = cancelText;

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
