using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;

public class QuestionManager
{
    private static Dictionary<string, QuestionSet> questionDict = new Dictionary<string, QuestionSet>();
    private const string questionSetFolderName = "QuestionSets";

    private static bool isInit = false;


    public static List<QuestionSet> GetAllQuestionSets()
    {
        TryInit();
        List<QuestionSet> allSetList = new List<QuestionSet>();
        foreach (string key in questionDict.Keys)
        {
            QuestionSet questionSet;
            if (questionDict.TryGetValue(key, out questionSet))
            {
                allSetList.Add(questionSet);
            }
            else
            {
                Debug.LogError("QuestionSet-Key had null set. (" + key + ")");
            }
        }

        return allSetList;
    }

    public static List<string> GetAllQuestionSetKeys()
    {
        TryInit();
        List<string> keys = new List<string>();
        foreach (string key in questionDict.Keys)
        {
            keys.Add(key);
        }

        return keys;
    }

    public static QuestionSet GetSet(string setName)
    {
        TryInit();
        QuestionSet questionSet;
        if (questionDict.TryGetValue(setName, out questionSet))
        {
            return questionSet;
        }

        return null;
    }

    public static void AddSet(string setName)
    {
        if (GetSet(setName) != null)
        {
            AddSet(setName + "_x"); // Make sure we don't add anything of the same name.
        }
        else
        {
            questionDict.Add(setName, new QuestionSet(setName));
        }
    }

    public static void RemoveSet(string setName)
    {
        if (GetSet(setName) != null)
        {
            questionDict.Remove(setName);
        }
    }

    private static void TryInit()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;
        string directoryPath = GetDirectoryPath();
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);

        }
        var info = new DirectoryInfo(directoryPath);
        var fileInfo = info.GetFiles();
        int nrSets = 0;
        foreach (FileInfo file in fileInfo)
        {
            QuestionSet questionSet;
            if (!questionDict.TryGetValue(file.Name, out questionSet))
            {
                string[] splitString = file.Name.Split('.');
                questionSet = new QuestionSet(splitString[0]);
                questionDict.Add(splitString[0], questionSet);
            }

            nrSets++;
        }

        if (nrSets == 0)
        {
            // Add a default one.
            const string defaultName = "Default";
            questionDict.Add(defaultName, new QuestionSet(defaultName));
        }
    }

    private static string GetDirectoryPath()
    {
        return Application.persistentDataPath + "/" + questionSetFolderName; ;
    }

    public class QuestionSet
    {
        private string keyName;
        private string fileName;
        private List<Question> currQuestionList = new List<Question>();

        public QuestionSet(string setName)
        {
            this.keyName = setName;
            this.fileName = GetDirectoryPath() + "/" + setName + ".qs";
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
            Clear();
            RemoveSet(keyName);
        }

        public void Clear()
        {
            if (!File.Exists(fileName))
            {
                Debug.Log("File didn't exist: " + fileName);
                return; // No questions to load.
            }
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
