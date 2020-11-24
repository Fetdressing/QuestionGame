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

        private IEnumerator autosaveIE = null;

        private void Awake()
        {
            QuestionManager.QuestionSet questionSet;
            clearSetButton.onClick.AddListener(() => { PromptClearCurrent(); });
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
            saveButton?.onClick.AddListener(() => { SaveCurrent(); });

            UpdateDropdown();
            questionSet = QuestionManager.GetSet(QuestionManager.GetAllSetNames()[0]);
            SetCurrent(questionSet, 0);
        }

        private QuestionManager.QuestionSet AddSet()
        {
            return QuestionManager.AddSet("MyNewSet");
        }

        private void SetCurrent(QuestionManager.QuestionSet questionSet, int dropdownIndex)
        {
            if (currSet != null)
            {
                SaveCurrent(); // Autosave.
            }

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

        private void PromptClearCurrent()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            ConfirmScreen.Create().Set("Are you sure you wish to clear this set?",
               confirm: () =>
               {
                   currSet.Clear();
                   questionDisplay.ClearQuestionGraphics();
               });           
        }

        private void PromptRemoveCurrent()
        {
            if (!CanRemoveCurrent())
            {
                return;
            }

            ConfirmScreen.Create().Set("Are you sure you wish to remove this set?",
                confirm: () =>
                {
                    currSet.Delete();
                    UpdateDropdown();
                    SetCurrent(null, 0);
                });
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

        #region Autosaving
        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                if (currSet != null)
                {
                    SaveCurrent(); // Autosave.
                }

                SetAutoSave(false);
            }
            else
            {
                SetAutoSave(true);
            }
        }

        private void OnApplicationQuit()
        {
            if (currSet != null)
            {
                SaveCurrent(); // Autosave.
            }
        }

        private void SetAutoSave(bool active)
        {
            if (autosaveIE != null)
            {
                StopCoroutine(autosaveIE);
            }

            if (active)
            {
                autosaveIE = AutoSaveIE();
                StartCoroutine(autosaveIE);
            }
        }

        private IEnumerator AutoSaveIE()
        {
            while (this != null)
            {
                yield return new WaitForSeconds(15);
                if (currSet != null)
                {
                    SaveCurrent();
                }
            }
        }
        #endregion
    }
}
