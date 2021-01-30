using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Edit
{
    public class QuestionDisplayerUI : MonoBehaviour
    {
        [Header("Scene Refs")]

        [SerializeField]
        private RectTransform questionRoot;

        [SerializeField]
        private Button addQuestionButton;

        [SerializeField]
        private ToggleInterface hideQuestionsToggle;

        [Header("Prefabs")]
        [SerializeField]
        private QuestionEditInterface questionPrefab;

        private QuestionManager.QuestionSet currSet;

        private static List<QuestionEditInterface> currentInterfaceList = new List<QuestionEditInterface>();

        private static bool showQuestionTexts = true;
        public static bool ShowQuestionTexts
        {
            get
            {
                return showQuestionTexts;
            }

            set
            {
                showQuestionTexts = value;
                for (int i = 0; i < currentInterfaceList.Count; i++)
                {
                    currentInterfaceList[i].SetVisibleText(value);
                }
            }
        }

        private void Awake()
        {
            addQuestionButton.onClick.AddListener(() => { AddQuestion(); });
            hideQuestionsToggle.Set(ShowQuestionTexts, (newValue) => { ShowQuestionTexts = newValue; });
        }

        public void SetCurrentSet(QuestionManager.QuestionSet questionSet)
        {
            currSet = questionSet;
            ClearQuestionGraphics();

            if (questionSet != null)
            {
                for (int i = 0; i < questionSet.GetQuestions().Count; i++)
                {
                    AddQuestionGraphics(questionSet.GetQuestions()[i], i * 0.06f);
                }
            }
        }

        private void AddQuestion()
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            QuestionManager.Question newQuestion = new QuestionManager.Question(QuestionManager.emptyQuestion);
            currSet.Add(newQuestion);
            AddQuestionGraphics(newQuestion);
        }

        private void RemoveQuestion(QuestionEditInterface questionUI)
        {
            if (currSet == null)
            {
                Debug.LogError(this.GetType().FullName + ": Null set.");
                return;
            }

            if (currSet.GetQuestions().Count <= 1)
            {
                ConfirmScreen.Create().Set("Can't remove! You must have atleast one question in the set.", useCancel: false);
                return;
            }

            ConfirmScreen.Create(UIUtil.UIType.ConfirmScreenRemove).Set("Remove Question?",
               confirm: () =>
               {
                   currSet.Remove(questionUI.Question.uniqueID);
                   RemoveQuestionGraphics(questionUI);
               }, confirmText: "Remove");
        }

        #region Graphics

        private void AddQuestionGraphics(QuestionManager.Question question, float delayedActivasion = -1f)
        {
            GameObject ob = Instantiate(questionPrefab.gameObject);
            QuestionEditInterface eInterface = ob.GetComponent<QuestionEditInterface>();
            eInterface.Set(question, RemoveQuestion);
            ob.transform.SetParent(questionRoot);
            ob.transform.localScale = Vector3.one;
            currentInterfaceList.Add(eInterface);

            if (delayedActivasion > 0f)
            {
                ob.SetActive(false);
                UIUtil.InvokeDelayed(() => { ob.SetActive(true); }, delayedActivasion);
            }
        }

        private void RemoveQuestionGraphics(QuestionEditInterface questionUI)
        {
            if (questionUI != null)
            {
                Destroy(questionUI.gameObject);
            }
        }

        public void ClearQuestionGraphics()
        {
            currentInterfaceList.Clear();
            foreach (Transform t in questionRoot.GetComponentsInChildren<Transform>())
            {
                if (t != questionRoot)
                {
                    Destroy(t.gameObject);
                }
            }
        }
        #endregion
    }
}
