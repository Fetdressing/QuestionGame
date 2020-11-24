using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ConfirmScreen : MonoBehaviour
{
    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private TextMeshProUGUI displayText;

    public void Set(string displayText = null, System.Action confirm = null, System.Action cancel = null)
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

        confirmButton.gameObject.SetActive(confirm != null);
        cancelButton.gameObject.SetActive(cancel != null);

        confirmButton.onClick.AddListener(() => { confirm.Invoke(); OnConfirm(); });
        cancelButton.onClick.AddListener(() => { cancel.Invoke(); OnCancel(); });
    }

    protected void OnConfirm()
    {
        DestroyScreen();
    }

    protected void OnCancel()
    {
        DestroyScreen();
    }

    protected void DestroyScreen()
    {
        Destroy(this.gameObject);
    }
}
