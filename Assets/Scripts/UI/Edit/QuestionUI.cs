using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

namespace Edit
{
    public class QuestionUI : MonoBehaviour
    {
        private QuestionManager.Question question;

        [SerializeField]
        private InputField questionInput;

        [SerializeField]
        private Button removeButton;

        public void Set(QuestionManager.Question question, System.Action<QuestionUI> removeCallback)
        {
            this.question = question;
            this.questionInput.SetTextWithoutNotify(question.value);
            this.questionInput.onValueChanged.AddListener((newValue) => { this.question.value = newValue; });
            this.removeButton.onClick.AddListener(() => { removeCallback.Invoke(this); });
        }

        public QuestionManager.Question Question
        {
            get
            {
                return this.question;
            }
        }
    }
}
