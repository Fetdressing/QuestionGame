using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Play
{
    public class SetSelectionInterface : MonoBehaviour
    {
        private QuestionManager.QuestionSet questionSet;

        [SerializeField]
        private TextMeshProUGUI questionSetNameText;

        [SerializeField]
        private Button toggleButton;

        [SerializeField]
        private RectTransform[] onGraphics;

        [SerializeField]
        private RectTransform[] offGraphics;

        private bool isOn;

        private void Awake()
        {
            toggleButton.onClick.AddListener(delegate { this.IsActive = !this.IsActive; });
        }

        public void Set(QuestionManager.QuestionSet questionSet, bool toggled)
        {
            this.questionSet = questionSet;

            this.questionSetNameText.text = this.questionSet.GetDisplayName();
            this.IsActive = toggled;
        }

        public bool IsActive
        {
            get
            {
                return this.isOn;
            }

            private set
            {
                this.isOn = value;

                for (int i = 0; i < onGraphics.Length; i++)
                {
                    onGraphics[i].gameObject.SetActive(this.isOn);
                }

                for (int i = 0; i < offGraphics.Length; i++)
                {
                    offGraphics[i].gameObject.SetActive(!this.isOn);
                }
            }
        }
    }
}
