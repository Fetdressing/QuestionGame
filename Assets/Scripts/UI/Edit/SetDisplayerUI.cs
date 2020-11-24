using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Edit
{
    public class SetDisplayerUI : MonoBehaviour
    {
        private QuestionManager.QuestionSet currSet;

        [SerializeField]
        private QuestionDisplayerUI questionDisplay;

        [SerializeField]
        private Dropdown questionSetDropdown;

        [SerializeField]
        private TMP_InputField questionSetNameInputField;

        [SerializeField]
        private Button saveButton;

        [SerializeField]
        private Button clearSetButton;

        private void Awake()
        {
            QuestionManager.QuestionSet questionSet;
            clearSetButton.onClick.AddListener(() => { ClearCurrent(); });
            questionSetDropdown.onValueChanged.AddListener(delegate
            {
                if (questionSetDropdown.value >= questionSetDropdown.options.Count - 1)
                {
                // Picked the last option.
                questionSet = AddSet();
                    UpdateDropdown();
                    SetCurrent(questionSet, questionSetDropdown.options.Count - 2);
                }
                else
                {
                    questionSet = QuestionManager.GetSet(QuestionManager.GetAllSetNames()[questionSetDropdown.value]);
                    SetCurrent(questionSet, questionSetDropdown.value);
                }
            });

            questionSetNameInputField.onSubmit.AddListener(OnSetNameChanged);
            saveButton.onClick.AddListener(() => { SaveCurrent(); });

            UpdateDropdown();
            questionSet = QuestionManager.GetSet(QuestionManager.GetAllSetNames()[0]);
            SetCurrent(questionSet, 0);
        }

        private QuestionManager.QuestionSet AddSet()
        {
            return QuestionManager.AddSet("New Set");
        }

        private void SetCurrent(QuestionManager.QuestionSet questionSet, int dropdownIndex)
        {
            currSet = questionSet;
            questionSetDropdown.SetValueWithoutNotify(dropdownIndex);
            questionSetNameInputField.SetTextWithoutNotify(questionSet.GetDisplayName());
            questionDisplay.SetCurrentSet(currSet);
        }

        private void SaveCurrent()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            currSet.Save();
        }

        private void ClearCurrent()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            currSet.Clear();
            questionDisplay.ClearQuestionGraphics();
        }

        private void RemoveCurrent()
        {
            if (!CanRemoveCurrent())
            {
                return;
            }

            currSet.Delete();
            UpdateDropdown();
            SetCurrent(null, 0);
        }

        private bool CanRemoveCurrent()
        {
            return currSet != null && QuestionManager.GetAllSetNames().Count > 1;
        }

        private void UpdateDropdown()
        {
            questionSetDropdown.ClearOptions();
            List<string> questionSetKeyList = QuestionManager.GetAllSetNames();
            questionSetDropdown.AddOptions(questionSetKeyList);
            questionSetDropdown.AddOptions(new List<string>() { "New Set..." });
        }

        private void OnSetNameChanged(string newName)
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            currSet.SetDisplayName(newName);
            UpdateDropdown();
        }
    }
}
