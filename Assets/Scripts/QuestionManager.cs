using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;

public class QuestionManager
{
    public const string emptyQuestion = "<Empty>";
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

#if UNITY_EDITOR
        if (UIUtil.HasDuplicate(names.ToArray()))
        {
            Debug.LogError("Make sure there are no duplicate names, we are expecting them to be unique.");
        }
#endif

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
            questionSet.Add(new Question("Give 5 drinks to X or X!")); // Add a default question.
            questionDict.Add(fileName, questionSet);
            return questionSet;
        }
    }

    public static void RemoveSet(string keyName)
    {
        if (questionDict.ContainsKey(keyName))
        {
            questionDict.Remove(keyName);
        }
        else
        {
            Debug.LogError("Couldn't find set to remove (" + keyName + ")");
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
                nrSets++;
            }
        }

        if (nrSets == 0)
        {
            // Add a default one.
            const string defaultName = "Default";
            AddSet(defaultName);
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
            if (GetAllSetNames().Contains(name))
            {
                SetDisplayNameUnique(name, 1);
            }
            else
            {
                this.displayName = name;
            }
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public List<Question> GetQuestions()
        {
            return currQuestionList;
        }

        public List<Question> GetPlayQuestions()
        {
            List<Question> playQuestionList = new List<Question>();
            playQuestionList.AddRange(currQuestionList);

            for (int i = playQuestionList.Count - 1; i >= 0; i--)
            {
                if (playQuestionList[i].value.Equals(QuestionManager.emptyQuestion))
                {
                    playQuestionList.RemoveAt(i);
                }
            }

            return playQuestionList;
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

            SetDisplayName(reader.ReadLine());

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
                Debug.LogError("File didn't exist: " + fileName);
                return; // No questions to load.
            }

            Clear();
            RemoveSet(keyName);
            File.Delete(fileName);
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

        private void SetDisplayNameUnique(string name, int currIndex)
        {
            string potName = name + "_" + currIndex.ToString();
            if (GetAllSetNames().Contains(potName))
            {
                SetDisplayNameUnique(name, currIndex + 1);
            }
            else
            {
                this.displayName = potName;
            }
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
