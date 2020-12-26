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
        private UnityButtonInterface setNameButton;

        [SerializeField]
        private Button saveButton;

        [SerializeField]
        private Button clearSetButton;

        [SerializeField]
        private Button removeSetButton;

        [SerializeField]
        private SceneButtonHandler sceneButtonHandler;

        private IEnumerator autosaveIE = null;

        private void Awake()
        {
            QuestionManager.QuestionSet questionSet;
            clearSetButton?.onClick.AddListener(() => { PromptClearCurrent(); });
            removeSetButton?.onClick.AddListener(PromptRemoveCurrent);
            questionSetDropdown.onValueChanged.AddListener(OnDropDownChanged);
            
            setNameButton.onClick.AddListener(PromptNameChange);
            saveButton?.onClick.AddListener(SaveCurrent);
            sceneButtonHandler.OnSceneChanged = SaveCurrent;

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
            setNameButton.Text = questionSet.GetDisplayName();
            questionDisplay.SetCurrentSet(currSet);
        }

        private void SaveCurrent()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

#if UNITY_EDITOR
            Debug.Log(this.GetType().FullName + ": Saved..." + currSet.GetDisplayName());
#endif

            currSet.Save();
        }

        private void PromptClearCurrent()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            ConfirmScreen.Create(UIUtil.UIType.ConfirmScreenReverse).Set("Are you sure you wish to clear this set?",
               confirm: () =>
               {
                   currSet.Clear();
                   questionDisplay.ClearQuestionGraphics();
               });           
        }

        private void PromptRemoveCurrent()
        {
            if (QuestionManager.GetAllSetNames().Count <= 1)
            {
                ConfirmScreen.Create().Set("You can't remove this set because you must atleast have one.", null, useCancel: false);
                return;
            }

            if (!CanRemoveCurrent())
            {
                return;
            }

            ConfirmScreen.Create(UIUtil.UIType.ConfirmScreenRemove).Set("Are you sure you wish to remove the set: <b><color=red>" + currSet.GetDisplayName() + "</color></b>?",
                confirm: () =>
                {
                    currSet.Delete();
                    currSet = null; // So it won't be autosaved.
                    UpdateDropdown();
                    SetCurrent(QuestionManager.GetSet(QuestionManager.GetAllSetNames()[0]), 0);
                }, confirmText: "Remove");
        }

        private bool CanRemoveCurrent()
        {
            return currSet != null && QuestionManager.GetAllSetNames().Count > 1;
        }

        #region Drop Down

        private void OnDropDownChanged(int newIndex)
        {
            QuestionManager.QuestionSet questionSet;

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
        }

        private void UpdateDropdown(string selectedName = "")
        {
            questionSetDropdown.ClearOptions();
            List<string> questionSetKeyList = QuestionManager.GetAllSetNames();
            questionSetDropdown.AddOptions(questionSetKeyList);
            questionSetDropdown.AddOptions(new List<string>() { "-Add-" });

            if (!string.IsNullOrEmpty(selectedName))
            {
                for (int i = 0; i < questionSetDropdown.options.Count; i++)
                {
                    if (string.Equals(questionSetDropdown.options[i].text, selectedName))
                    {
                        questionSetDropdown.SetValueWithoutNotify(i);
                        break;
                    }
                }                
            }
        }

        #endregion

        private void PromptNameChange()
        {
            InputScreen.Create().Set("New Set Name?", 2, 15, true, callbackConfirm: OnSetNameChanged, lineType: InputField.LineType.SingleLine, startValue: currSet.GetDisplayName());
        }

        private void OnSetNameChanged(string newName)
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            if (string.IsNullOrEmpty(newName))
            {
                return;
            }

            currSet.SetDisplayName(newName);
            setNameButton.Text = newName;
            UpdateDropdown(newName);
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
