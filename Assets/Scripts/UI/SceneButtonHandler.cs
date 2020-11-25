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

        public void Init()
        {
            button?.onClick.AddListener(MoveToScene);
        }

        private void MoveToScene()
        {
            SceneManager.LoadScene(sceneGoToName, LoadSceneMode.Single);
        }
    }
}
