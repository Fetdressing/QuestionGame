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

        private bool visibleText;

        public void SetVisibleText(bool visible)
        {
            // Important order.
            visibleText = visible;
            string displayValue = ToDisplayString(this.question.value);
            this.editButton.Text = displayValue;
        }

        public void Set(QuestionManager.Question question, System.Action<QuestionEditInterface> removeCallback)
        {
            this.visibleText = QuestionDisplayerUI.ShowQuestionTexts;
            this.Set(question);
            this.editButton.onClick.AddListener(() => { PromptChangeQuestion(this); });
            this.removeButton.onClick.AddListener(() => { removeCallback.Invoke(this); });
            this.SetValue(question.value);
        }

        private void SetValue(string newValue)
        {
            this.question.value = newValue;
            string displayValue = ToDisplayString(newValue);
            this.editButton.Text = displayValue;
        }

        private void PromptChangeQuestion(QuestionEditInterface questionEditInterface)
        {
            if (questionEditInterface == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null interface.");
                return;
            }

            string startValue = string.Equals(questionEditInterface.Question.value, QuestionManager.emptyQuestion) ? "" : questionEditInterface.Question.value;
            InputScreen.Create().Set("Change question: ", 3, 70, true, (newValue) => { OnChangeQuestion(questionEditInterface, newValue); }, startValue: startValue);
        }

        private void OnChangeQuestion(QuestionEditInterface questionEditInterface, string newValue)
        {
            if (string.IsNullOrEmpty(newValue) || questionEditInterface == null)
            {
                return;
            }

            questionEditInterface.SetValue(newValue);
        }

        private string ToDisplayString(string value)
        {
            string displayValue;

            if (visibleText)
            {
                displayValue = UIUtil.ToColoredNamesString(value);
                const int maxDisplaySize = 45;

                if (displayValue.Length > maxDisplaySize)
                {
                    int indexToSplitAt = maxDisplaySize;
                    bool isWithinRichText = UIUtil.IsStringIndexWithinRichText(displayValue, indexToSplitAt);

                    if (isWithinRichText)
                    {
                        indexToSplitAt = UIUtil.FindIndexOutsideRichText(displayValue, maxDisplaySize);
                    }
                    else
                    {
                        char currChar = displayValue[indexToSplitAt];
                        while (currChar != ' ' && indexToSplitAt > 0) // Find closest space.
                        {
                            indexToSplitAt--;
                            currChar = displayValue[indexToSplitAt];
                        }
                    }

                    displayValue = displayValue.Substring(0, indexToSplitAt) + "...";
                }
            }
            else
            {
                displayValue = "...";
            }

            return displayValue;
        }
    }
}
