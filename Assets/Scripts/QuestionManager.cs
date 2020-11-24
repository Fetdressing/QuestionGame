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

    public static List<string> GetAllSetNames()
    {
        TryInit();
        List<string> names = new List<string>();
        foreach (string key in questionDict.Keys)
        {
            QuestionSet questionSet;
            if (questionDict.TryGetValue(key, out questionSet))
            {
                names.Add(questionSet.GetDisplayName());
            }
        }

        return names;
    }

    public static QuestionSet GetSet(string setName)
    {
        TryInit();

        foreach (string key in questionDict.Keys)
        {
            QuestionSet questionSet;
            if (questionDict.TryGetValue(key, out questionSet))
            {
                if (string.Equals(questionSet.GetDisplayName(), setName))
                {
                    return questionSet;
                }
            }
        }        

        return null;
    }

    public static QuestionSet AddSet(string setName)
    {
        string fileName = "S" + Random.Range(0, 1000000).ToString();
        if (GetSet(fileName) != null)
        {
            return AddSet(setName); // Make sure we don't add anything of the same name.
        }
        else
        {
            QuestionSet questionSet = new QuestionSet(fileName);
            questionSet.SetDisplayName(setName);
            questionSet.Add(new Question("Would you rather...")); // Add a default question.
            questionDict.Add(fileName, questionSet);
            return questionSet;
        }
    }

    public static void RemoveSet(string setName)
    {
        if (GetSet(setName) != null)
        {
            questionDict.Remove(setName);
        }
    }

    #region Private
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
    #endregion

    public class QuestionSet
    {
        private string displayName;
        private string keyName;
        private string fileName;
        private List<Question> currQuestionList = new List<Question>();

        public QuestionSet(string fileName)
        {            
            this.keyName = fileName;
            this.fileName = GetDirectoryPath() + "/" + fileName + ".qs";
            this.Load();
        }

        public void SetDisplayName(string name)
        {
            this.displayName = name;
        }

        public string GetDisplayName()
        {
            return displayName;
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

            sw.WriteLine(displayName);

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

            displayName = reader.ReadLine();

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
