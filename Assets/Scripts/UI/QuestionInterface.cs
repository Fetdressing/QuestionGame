using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class QuestionInterface : MonoBehaviour
{
    protected QuestionManager.Question question;

    public void Set(QuestionManager.Question question)
    {
        this.question = question;
    }

    public QuestionManager.Question Question
    {
        get
        {
            return this.question;
        }
    }
}
