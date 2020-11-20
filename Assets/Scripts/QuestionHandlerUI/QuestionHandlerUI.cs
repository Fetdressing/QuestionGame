using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class QuestionHandlerUI : MonoBehaviour
{
    [Header("Scene Refs")]

    [SerializeField]
    private RectTransform questionRoot;

    [SerializeField]
    private Dropdown questionSetDropdown;

    [SerializeField]
    private Button addQuestionButton;

    [SerializeField]
    private Button saveButton;

    [SerializeField]
    private Button clearSetButton;

    [Header("Prefabs")]
    [SerializeField]
    private QuestionUI questionPrefab;

    private QuestionManager.QuestionSet currSet;

    private void Awake()
    {
        addQuestionButton.onClick.AddListener(() => { AddQuestion(); });
        saveButton.onClick.AddListener(() => { Save(); });
        clearSetButton.onClick.AddListener(() => { ClearSet(); });

        UpdateQuestionSetDropdown();
        QuestionManager.QuestionSet questionSet = QuestionManager.GetSet(QuestionManager.GetAllQuestionSetKeys()[0]);
        Set(questionSet);
    }

    private void Set(QuestionManager.QuestionSet questionSet)
    {
        currSet = questionSet;

        for (int i = 0; i < questionSet.GetQuestions().Count; i++)
        {
            AddQuestionGraphics(questionSet.GetQuestions()[i]);
        }
    }

    private void AddSet()
    {

    }

    private void RemoveCurrentSet()
    {
        if (!CanRemoveCurrentSet())
        {
            return;
        }

        currSet.Delete();
        ClearQuestionGraphics();
        UpdateQuestionSetDropdown();
    }

    private bool CanRemoveCurrentSet()
    {
        return currSet != null && QuestionManager.GetAllQuestionSetKeys().Count > 1;
    }

    #region HandleSet
    private void Save()
    {
        if (currSet == null)
        {
            Debug.LogError(this.GetType().FullName + ": Null set.");
            return;
        }

        currSet.Save();
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

    private void RemoveQuestion(QuestionUI questionUI)
    {
        if (currSet == null)
        {
            Debug.LogError(this.GetType().FullName + ": Null set.");
            return;
        }

        currSet.Remove(questionUI.Question.uniqueID);
        RemoveQuestionGraphics(questionUI);
    }

    private void ClearSet()
    {
        if (currSet == null)
        {
            Debug.LogError(this.GetType().FullName + ": Null set.");
            return;
        }

        currSet.Clear();
        ClearQuestionGraphics();
    }
    #endregion

    #region Graphics

    private void UpdateQuestionSetDropdown()
    {
        questionSetDropdown.ClearOptions();
        List<string> questionSetKeyList = QuestionManager.GetAllQuestionSetKeys();
        questionSetDropdown.AddOptions(questionSetKeyList);
    }

    private void AddQuestionGraphics(QuestionManager.Question question)
    {
        GameObject ob = Instantiate(questionPrefab.gameObject);
        ob.GetComponent<QuestionUI>().Set(question, RemoveQuestion);
        ob.transform.SetParent(questionRoot);
    }

    private void RemoveQuestionGraphics(QuestionUI questionUI)
    {
        if (questionUI != null)
        {
            Destroy(questionUI.gameObject);
        }
    }

    private void ClearQuestionGraphics()
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
