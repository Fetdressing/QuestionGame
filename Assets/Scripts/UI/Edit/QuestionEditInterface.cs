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
        private UnityButtonInterface editButton;

        [SerializeField]
        private Button removeButton;

        public void Set(QuestionManager.Question question, System.Action<QuestionEditInterface> removeCallback)
        {
            this.Set(question);
            this.editButton.onClick.AddListener(() => { PromptChangeQuestion(this); });
            this.removeButton.onClick.AddListener(() => { removeCallback.Invoke(this); });
            this.SetValue(question.value, UIUtil.ToColoredNamesString(question.value));
        }

        public void SetValue(string newValue, string displayValue)
        {
            this.question.value = newValue;
            this.editButton.Text = displayValue;
        }

        private void PromptChangeQuestion(QuestionEditInterface questionEditInterface)
        {
            if (questionEditInterface == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null interface.");
                return;
            }

            InputScreen.Create().Set("Change question: ", 3, 50, true, (newValue) => { OnChangeQuestion(questionEditInterface, newValue); }, startValue: questionEditInterface.Question.value);
        }

        private void OnChangeQuestion(QuestionEditInterface questionEditInterface, string newValue)
        {
            if (string.IsNullOrEmpty(newValue) || questionEditInterface == null)
            {
                return;
            }

            questionEditInterface.SetValue(newValue, UIUtil.ToColoredNamesString(newValue));
        }
    }
}
