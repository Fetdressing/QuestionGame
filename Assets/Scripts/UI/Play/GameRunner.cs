using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace Play
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private UnityButtonInterface nextButton;

        [SerializeField]
        private TextMeshProUGUI questionDisplayer;

        [SerializeField]
        private Animator animH;
        private const string showC = "show";
        private float timeNextButtonActive = 0f;
        
        List<QuestionManager.Question> questionList = new List<QuestionManager.Question>();

        private void Awake()
        {
            nextButton.onClick.AddListener(() =>
            {
                if (IsNextButtonReady())
                {
                    AnimateNextQuestion();
                }
            });
        }

        private void OnEnable()
        {
            Replay();
        }

        private void Replay()
        {
            questionList.Clear();
            for (int i = 0; i < SetSelection.GetSets().Count; i++)
            {
                questionList.AddRange(SetSelection.GetSets()[i].GetPlayQuestions());
            }

            AnimateNextQuestion();
        }

        private void AnimateNextQuestion()
        {            
            animH.SetBool(showC, false);
            UIUtil.InvokeDelayed(() => 
            {                
                DisplayNextQuestion();
            }, 0.5f);

            timeNextButtonActive = Time.time + 0.7f;
        }

        private bool IsNextButtonReady()
        {
            return timeNextButtonActive < Time.time;
        }

        private void DisplayNextQuestion()
        {
            if (questionList.Count == 0)
            {
                ConfirmScreen.Create().Set("Play Again?", Replay, PlayHandler.BackPhase, useCancel: true, confirmText: "Replay");
                return;
            }

            animH.SetBool(showC, true);

            int index = Random.Range(0, questionList.Count);
            QuestionManager.Question question = questionList[index];
            questionList.RemoveAt(index);

            questionDisplayer.text = UIUtil.ToColoredNamesString(question.value, PlayerSelection.GetPlayers());
        }
    }
}
