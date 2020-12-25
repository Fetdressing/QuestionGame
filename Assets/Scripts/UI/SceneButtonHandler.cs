using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;

public class SceneButtonHandler : MonoBehaviour
{
    [SerializeField]
    private ButtonScene[] buttons;

    private System.Action onSceneChanged;

    public System.Action OnSceneChanged
    {
        get
        {
            return onSceneChanged;
        }

        set
        {
            onSceneChanged = value;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].OnSceneChanged = onSceneChanged;
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Init();
        }
    }

    [System.Serializable]
    private class ButtonScene
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private string sceneGoToName;

        [SerializeField]
        private string prompt = null;

        public System.Action OnSceneChanged { get; set; } = null;

        public void Init()
        {
            button?.onClick.AddListener(MoveToScene);
        }

        private void MoveToScene()
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                ConfirmScreen.Create().Set(prompt, OnConfirm, confirmText: "Main Menu");
            }
            else
            {
                OnConfirm();
            }            
        }

        private void OnConfirm()
        {
            if (OnSceneChanged != null)
            {
                OnSceneChanged.Invoke();
            }

            SceneManager.LoadScene(sceneGoToName, LoadSceneMode.Single);
        }
    }
}
