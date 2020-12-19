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

        [Header("Prefabs")]
        [SerializeField]
        private QuestionEditInterface questionPrefab;

        private QuestionManager.QuestionSet currSet;

        private void Awake()
        {
            addQuestionButton.onClick.AddListener(() => { AddQuestion(); });
        }

        public void SetCurrentSet(QuestionManager.QuestionSet questionSet)
        {
            currSet = questionSet;
            ClearQuestionGraphics();

            if (questionSet != null)
            {
                for (int i = 0; i < questionSet.GetQuestions().Count; i++)
                {
                    AddQuestionGraphics(questionSet.GetQuestions()[i]);
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

            QuestionManager.Question newQuestion = new QuestionManager.Question("New Question...");
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

            ConfirmScreen.Create().Set("Are you sure you wish to remove this question?",
               confirm: () =>
               {
                   currSet.Remove(questionUI.Question.uniqueID);
                   RemoveQuestionGraphics(questionUI);
               });
        }

        #region Graphics


        private void AddQuestionGraphics(QuestionManager.Question question)
        {
            GameObject ob = Instantiate(questionPrefab.gameObject);
            ob.GetComponent<QuestionEditInterface>().Set(question, RemoveQuestion);
            ob.transform.SetParent(questionRoot);
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
