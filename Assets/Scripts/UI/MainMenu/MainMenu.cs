using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private ButtonScene playButton;

    [SerializeField]
    private ButtonScene editButton;

    private void Awake()
    {
        playButton.Init();
        editButton.Init();
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
            button.onClick.AddListener(MoveToScene);
        }

        private void MoveToScene()
        {
            SceneManager.LoadScene(sceneGoToName, LoadSceneMode.Single);
        }
    }
}
