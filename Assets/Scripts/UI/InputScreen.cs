using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class InputScreen : ConfirmScreen
{
    /// <summary>
    /// What should happen when it is confirmed.
    /// </summary>
    public System.Action<string> callbackConfirm = null;

    /// <summary>
    /// An input field.
    /// </summary>
    public UnityInputFieldInterface input;

    /// <summary>
    /// The minimum amount of letters in acceptable input.
    /// </summary>
    protected int minTextLength = 0;

    /// <summary>
    /// The maximum amount of letters in acceptable input.
    /// </summary>
    protected int maxTextLength = 50;

    /// <summary>
    /// The normal error saying that the input is to short/long. Is set in the set method.
    /// </summary>
    protected string defaultErrorDisplayText;

    /// <summary>
    /// The start input value.
    /// </summary>
    protected string startValue;

    /// <summary>
    /// The text object that shows the required amount of letters.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI requirementsText;

    public static InputScreen Create()
    {
        return (InputScreen)UIUtil.Spawn(UIUtil.UIType.InputScreen);
    }

    /// <summary>
    /// Only allows confirming when the text is within acceptable limits.
    /// </summary>
    public void OnInputChange()
    {
        this.input.text = System.Text.RegularExpressions.Regex.Replace(this.input.text, @"[^A-Za-z _/()!"".\-,:+= &#?0-9@£$€{[\]}]", "");

        bool isValid = IsValidInput(out string errorMessage);

        if (isValid)
        {
            confirmButton.gameObject.SetActive(true);
            requirementsText.gameObject.SetActive(false);
        }
        else
        {
            requirementsText.text = errorMessage;
            confirmButton.gameObject.SetActive(false);
            requirementsText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Displays this screen.
    /// </summary>
    /// <param name="infoText">The text that should be displayed in the screen.</param>
    /// <param name="minTextLength">The lowest acceptable letter-count in input.</param>
    /// <param name="maxTextLength">The maximum acceptable letter-count in input.</param>
    /// <param name="allowCancel">Whether to allow cancelling or not.</param>
    /// <param name="callbackConfirm">Callback for what happens when confirming.</param>
    /// <param name="keyboardType">The type of keyboard to be displayed when inputting. Should preferably match the content type.</param>
    /// <param name="startValue">Start text value.</param>
    /// <param name="inputType">The input type.</param>
    /// <param name="lineType">The line type.</param>
    public virtual void Set(string infoText, int minTextLength, int maxTextLength, bool allowCancel, System.Action<string> callbackConfirm = null, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, string startValue = "", InputField.LineType lineType = InputField.LineType.SingleLine, InputField.ContentType contentType = InputField.ContentType.Standard, string promptText = "Enter Text")
    {
        this.displayText.text = infoText;
        this.startValue = startValue;
        this.minTextLength = minTextLength;
        this.maxTextLength = maxTextLength;
        this.defaultErrorDisplayText = "Must be between " + minTextLength.ToString() + " and " + maxTextLength.ToString() + " characters long";

        this.input.onValueChanged.AddListener(delegate { OnInputChange(); });
        this.input.keyboardType = keyboardType;
        this.input.colors = GetColorBlock();
        this.input.contentType = contentType;
        this.input.lineType = lineType;
        this.input.characterLimit = maxTextLength;
        this.input.SetTextWithoutNotify(startValue);
        this.input.placeholder.GetComponent<Text>().text = promptText;
        this.input.Select();
        OnInputChange(); // Makes sure buttons and requirements text is properly enabled/disabled.

        this.cancelButton.gameObject.SetActive(allowCancel);

        this.callbackConfirm = callbackConfirm;
    }

    /// <summary>
    /// Method for confirming.
    /// </summary>
    public virtual void Confirm()
    {
        if (callbackConfirm != null)
        {
            callbackConfirm.Invoke(this.GetInputText());
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Method for canceling.
    /// </summary>
    public virtual void Cancel()
    {
        if (callbackConfirm != null)
        {
            callbackConfirm.Invoke(null);
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Gets whether the current input is valid or not.
    /// </summary>
    /// <param name="errorMessage">The error message to be displayed if the input is not valid.</param>
    /// <returns>Returns true if the input is valid.</returns>
    public virtual bool IsValidInput(out string errorMessage)
    {
        bool isValid = true;
        errorMessage = "";

        string inputText = this.GetInputText();

        if (inputText.Length < this.minTextLength || inputText.Length > this.maxTextLength)
        {
            errorMessage = defaultErrorDisplayText;
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Gets the input text.
    /// </summary>
    /// <returns>The input text.</returns>
    public virtual string GetInputText()
    {
        return this.input.text.Trim();
    }

    /// <summary>
    /// On initialization.
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Cancel);
    }

    /// <summary>
    /// Gets the color block to be used for input screens.
    /// </summary>
    /// <returns>Returns the color block.</returns>
    protected ColorBlock GetColorBlock()
    {
        ColorBlock colorBlock = ColorBlock.defaultColorBlock;
        Color selColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        colorBlock.disabledColor = selColor;
        colorBlock.pressedColor = selColor;
        colorBlock.selectedColor = selColor;
        colorBlock.highlightedColor = selColor;
        colorBlock.normalColor = Color.white;
        return colorBlock;
    }
}
