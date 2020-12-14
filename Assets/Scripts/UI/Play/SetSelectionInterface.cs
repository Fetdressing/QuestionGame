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
        private System.Action onChanged;

        private void Awake()
        {
            toggleButton.onClick.AddListener(delegate { this.IsOn = !this.IsOn; });
        }

        public void Set(QuestionManager.QuestionSet questionSet, bool toggled, System.Action onChanged = null)
        {
            this.questionSet = questionSet;

            this.questionSetNameText.text = this.questionSet.GetDisplayName();
            this.IsOn = toggled;
            this.onChanged = onChanged;
        }

        public QuestionManager.QuestionSet Get()
        {
            return this.questionSet;
        }

        public bool IsOn
        {
            get
            {
                return this.isOn;
            }

            private set
            {
                bool oldVal = this.isOn;
                this.isOn = value;

                for (int i = 0; i < onGraphics.Length; i++)
                {
                    onGraphics[i].gameObject.SetActive(this.isOn);
                }

                for (int i = 0; i < offGraphics.Length; i++)
                {
                    offGraphics[i].gameObject.SetActive(!this.isOn);
                }

                if (oldVal != value)
                {
                    this.onChanged?.Invoke();
                }
            }
        }
    }
}
