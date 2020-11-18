using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    private List<string> currQuestionList = new List<string>();

    public void Add()
    {

    }

    public void Remove(int index)
    {

    }

    public List<string> GetQuestions()
    {
        return currQuestionList;
    }

    public void Save(string fileName)
    {
        StreamWriter sw = new StreamWriter(fileName, true);
        for (int i = 0; i < currQuestionList.Count; i++)
        {
            sw.Write(currQuestionList[i]);
        }

        sw.Close();
    }

    public void Load(string fileName)
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
            currQuestionList.Add(question);
        }

        reader.Close();
    }
}
