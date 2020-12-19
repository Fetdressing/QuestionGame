using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using TMPro;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    protected RectTransform graphicBase;

    [SerializeField]
    private TextMeshProUGUI defaultText;

    public string Text
    {
        set
        {
            if (this.defaultText != null)
            {
                this.defaultText.text = value;
            }
        }
    }

    protected virtual void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void OnEditorUpdate()
    {
        defaultText = this.GetComponent<TextMeshProUGUI>();
    }

    protected bool EditorCanUpdate()
    {
        return !UIUtil.IsPrefab(this) && Application.isPlaying == false;
    }

    private void Awake()
    {
        OnInit();
    }

    private void RunEditorUpdate()
    {
        if (EditorCanUpdate())
        {
            OnEditorUpdate();
        }
    }
    
    [UnityEditor.InitializeOnLoad]
    [UnityEditor.CustomEditor(typeof(UIBase), true)]
    public class UIBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIBase t = (UIBase)target;

            if (GUILayout.Button("Update"))
            {
                t.RunEditorUpdate();
            }
        }
    }
}
