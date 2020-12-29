using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Used for interfacing with <see cref="InputField"/>.
/// </summary>
public class UnityInputFieldInterface : UnityEngine.UI.InputField
{
    /// <summary>
    /// Sound to play when clicking the object.
    /// </summary>
    [SerializeField]
    private AudioManager.EffectType onClickSound = AudioManager.EffectType.Click1;

    /// <summary>
    /// The click force.
    /// </summary>
    [SerializeField]
    private float clickForce = 0.6f;

    /// <summary>
    /// Whether to apply click effect on children or not.
    /// </summary>
    [SerializeField]
    private bool clickApplyChildren = false;

    [SerializeField]
    private bool clickApplyTargetGraphic = true;

    /// <summary>
    /// Children of this.
    /// </summary>
    [SerializeField]
    private Transform[] children;

    /// <summary>
    /// On pointer enter.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        SetTargetGraphicScale(1 - (0.2f * clickForce));
    }

    /// <summary>
    /// On pointer exit.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        SetTargetGraphicScale(1);
    }

    /// <summary>
    /// On select.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        this.OnClick();
    }

    /// <summary>
    /// On deselect.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        this.OnClick();
    }

    /// <summary>
    /// Sets whether this should be interactable or not.
    /// </summary>
    /// <param name="interactable">True to be interactable.</param>
    public void SetInteractable(bool interactable)
    {
        this.enabled = interactable;
        Color targetCol = this.targetGraphic.color;
        this.targetGraphic.color = new Color(targetCol.r, targetCol.g, targetCol.b, interactable ? 1f : 0.3f);
    }

    /// <summary>
    /// On clicking button.
    /// </summary>
    protected void OnClick()
    {
        AudioManager.Play(onClickSound);
        SetTargetGraphicScale(1);
    }

    /// <summary>
    /// On enable.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        SetTargetGraphicScale(1);
    }

#if UNITY_EDITOR
    /// <summary>
    /// On validate.
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        children = Utility.GetFirstLevelChildren(this.transform).ToArray();
    }
#endif

    /// <summary>
    /// On application changed focus.
    /// </summary>
    /// <param name="focus">The new focus.</param>
    private void OnApplicationFocus(bool focus)
    {
        if (Application.isPlaying && this.gameObject.activeInHierarchy && this.enabled)
        {
            // Leaving touch screen code here because we might need it soon.
            ////TouchScreenKeyboard.Status currStatus;

            ////if (this.touchScreenKeyboard != null)
            ////{
            ////    currStatus = this.touchScreenKeyboard.status;
            ////}
            ////else
            ////{
            ////    currStatus = TouchScreenKeyboard.Status.Canceled;
            ////}

            if (focus)
            {
                // Keyboard stopped being visible, invoke that we have deselected.
                this.OnDeselect(new BaseEventData(EventSystem.current));
            }
        }
    }

    /// <summary>
    /// Sets the target graphic scale.
    /// </summary>
    /// <param name="scale">Scale to set.</param>
    private void SetTargetGraphicScale(float scale)
    {
        if (targetGraphic != null && clickApplyTargetGraphic)
        {
            targetGraphic.transform.localScale = new Vector3(scale, scale, 1);
        }

        if (children != null && clickApplyChildren)
        {
            for (int i = 0; i < children.Length; i++)
            {
                children[i].transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Class for drawing the new button.
    /// </summary>
    [CustomEditor(typeof(UnityInputFieldInterface))]
    public class ButtonEditorDraw : UnityEditor.UI.InputFieldEditor
    {
        /// <summary>
        /// On drawing UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UnityInputFieldInterface targetInterface = (UnityInputFieldInterface)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickApplyTargetGraphic"), new GUIContent("Click Apply Target", "Whether to apply click effect to target graphic or not."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickApplyChildren"), new GUIContent("Click Apply Children", "Whether to apply click effect to children or not."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onClickSound"), new GUIContent("Click sound", "Sound to play when clicked."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickForce"), new GUIContent("Click Force", "What force the click effect should have."));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
