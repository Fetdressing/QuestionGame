using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;

public class QuestionManager
{
    private static Dictionary<string, QuestionSet> questionDict = new Dictionary<string, QuestionSet>();

    public static void SaveQuestionSet(string setName)
    {
        GetSet(setName).Save();
    }

    public static QuestionSet GetSet(string setName)
    {
        QuestionSet questionSet;
        if (!questionDict.TryGetValue(setName, out questionSet))
        {
            questionSet = new QuestionSet(setName);
        }

        return questionSet;
    }

    public class QuestionSet
    {
        private string fileName;
        private List<Question> currQuestionList = new List<Question>();

        public QuestionSet(string setName)
        {
            this.fileName = Application.persistentDataPath + "/setName";
            this.Load();
        }

        public List<Question> GetQuestions()
        {
            return currQuestionList;
        }

        public void Add(Question question)
        {
            currQuestionList.Add(question);
        }

        public void Remove(int uniqueID)
        {
            for (int i = 0; i < currQuestionList.Count; i++)
            {
                if (currQuestionList[i].uniqueID == uniqueID)
                {
                    currQuestionList.RemoveAt(i);
                    return;
                }
            }

            Debug.LogError(this.GetType().FullName + ": Couldn't find unique ID (" + uniqueID + ").");
        }

        public void Save()
        {
            StreamWriter sw = new StreamWriter(fileName, false);

            for (int i = 0; i < currQuestionList.Count; i++)
            {
                sw.WriteLine(currQuestionList[i].value);
            }

            sw.Close();
        }

        public void Load()
        {
            if (!File.Exists(fileName))
            {
                Debug.Log("File didn't exist: " + fileName);
                return; // No questions to load.
            }

            currQuestionList.Clear();
            StreamReader reader = new StreamReader(fileName);

            while (!reader.EndOfStream)
            {
                string question = reader.ReadLine();
                Add(new Question(question));
            }

            reader.Close();
        }

        public void Delete()
        {
            if (!File.Exists(fileName))
            {
                Debug.Log("File didn't exist: " + fileName);
                return; // No questions to load.
            }

            File.Delete(fileName);
            currQuestionList.Clear();
        }
    }

    public class Question
    {
        private static int currID = 0;

        public string value;
        public int uniqueID;

        public Question(string value)
        {
            this.value = value;
            uniqueID = currID;
            currID++;
        }
    }
}
