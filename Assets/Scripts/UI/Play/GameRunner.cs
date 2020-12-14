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

        List<string> playerList;
        List<QuestionManager.Question> questionList = new List<QuestionManager.Question>();

        private void Awake()
        {
            nextButton.onClick.AddListener(DisplayNextQuestion);
        }

        private void OnEnable()
        {
            playerList = PlayerSelection.GetPlayers();
            Replay();
        }

        private void Replay()
        {
            questionList.Clear();
            for (int i = 0; i < SetSelection.GetSets().Count; i++)
            {
                questionList.AddRange(SetSelection.GetSets()[i].GetQuestions());
            }

            DisplayNextQuestion();
        }

        private void DisplayNextQuestion()
        {
            if (questionList.Count == 0)
            {
                Debug.Log("Replaying questions...");
                Replay();
                return;
            }

            int index = Random.Range(0, questionList.Count);
            QuestionManager.Question question = questionList[index];
            questionList.RemoveAt(index);

            questionDisplayer.text = question.value;
        }
    }
}
