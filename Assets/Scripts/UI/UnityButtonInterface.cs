using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

/// <summary>
/// Used for interfacing with <see cref="Button"/>.
/// </summary>
public class UnityButtonInterface : UnityEngine.UI.Button
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
    private float clickForce = 0.3f;

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

    [SerializeField]
    private TextMeshProUGUI defaultText;

    public string Text
    {
        get
        {
            return this.defaultText == null ? "" : this.defaultText.text;
        }

        set
        {
            if (this.defaultText != null)
            {
                this.defaultText.text = value;
            }
        }
    }

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
    /// On clicking button.
    /// </summary>
    protected void OnClick()
    {
        AudioManager.Play(onClickSound);
        SetTargetGraphicScale(1);
    }

    /// <summary>
    /// On awake.
    /// </summary>
    protected override void Awake()
    {
        this.onClick.AddListener(OnClick);
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
    [CustomEditor(typeof(UnityButtonInterface))]
    public class ButtonEditorDraw : UnityEditor.UI.ButtonEditor
    {
        /// <summary>
        /// On drawing UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UnityButtonInterface targetInterface = (UnityButtonInterface)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickApplyTargetGraphic"), new GUIContent("Click Apply Target", "Whether to apply click effect to target graphic or not."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickApplyChildren"), new GUIContent("Click Apply Children", "Whether to apply click effect to children or not."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onClickSound"), new GUIContent("Click sound", "Sound to play when clicked."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickForce"), new GUIContent("Click Force", "What force the click effect should have."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultText"), new GUIContent("Default text that can be used."));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
