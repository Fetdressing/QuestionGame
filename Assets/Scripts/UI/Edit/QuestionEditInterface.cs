using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

namespace Edit
{
    public class QuestionEditInterface : QuestionInterface
    {
        [SerializeField]
        private UnityInputFieldInterface questionInput;

        [SerializeField]
        private Button removeButton;

        public void Set(QuestionManager.Question question, System.Action<QuestionEditInterface> removeCallback)
        {
            this.Set(question);
            this.questionInput.SetTextWithoutNotify(question.value);
            this.questionInput.onValueChanged.AddListener((newValue) => { this.question.value = newValue; });
            this.removeButton.onClick.AddListener(() => { removeCallback.Invoke(this); });
        }
    }
}
