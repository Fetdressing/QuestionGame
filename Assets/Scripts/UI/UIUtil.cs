using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using RotaryHeart.Lib.SerializableDictionary;
using RotaryHeart;

public class UIUtil : MonoBehaviour
{
    private static UIUtil instance;

    [SerializeField]
    private UIDict uiDict;

    [SerializeField]
    private List<Color32> playerColorList = new List<Color32>();
    private string[] toSplitBy = new string[] { " x", " X", "x ", "X " };

    public enum UIType
    {
        ConfirmScreen,
        ConfirmScreenRemove,
        ConfirmScreenReverse,
        InputScreen
    }

    private static UIUtil Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIUtil>();
                DontDestroyOnLoad(instance.gameObject);
            }

            UIUtil[] potInstances = FindObjectsOfType<UIUtil>();

            for (int i = potInstances.Length - 1; i >= 0; i--)
            {
                if (potInstances[i] != instance)
                {
                    Destroy(potInstances[i].gameObject);
                }
            }

            return instance;
        }
    }

    public static UIBase Spawn(UIType uiType)
    {
        UIBase uiBase;
        if (Instance.uiDict.TryGetValue(uiType, out uiBase))
        {
            GameObject ob = Instantiate(uiBase.gameObject);
            return ob.GetComponent<UIBase>();
        }
        else
        {
            Debug.LogError("Null prefab: " + uiType.ToString());
            return null;
        }
    }

    public static T[] GetAllObjectsInScene<T>() where T : MonoBehaviour
    {
        List<T> objectsInScene = new List<T>();

        T[] unfiliteredArray = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

        foreach (T t in unfiliteredArray)
        {
            if (IsPrefab(t))
            {
                continue;
            }

            objectsInScene.Add(t);
        }

        return objectsInScene.ToArray();
    }

    public static bool HasDuplicate<T>(T[] array) where T : class
    {
        List<T> searchedList = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (searchedList.Contains(array[i]))
            {
                return true;
            }

            for (int y = 0; y < searchedList.Count; y++)
            {
                if (searchedList[y] == array[i])
                {
                    return true;
                }
            }

            searchedList.Add(array[i]);
        }

        return false;
    }

    public static bool IsPrefab(MonoBehaviour obj)
    {
        // During runtime.
        if (UnityEngine.Application.isPlaying)
        {
            if (obj.gameObject.hideFlags == HideFlags.NotEditable || obj.gameObject.hideFlags == HideFlags.HideAndDontSave)
            {
                return true;
            }

            if (!obj.gameObject.scene.isLoaded)
            {
                return true;
            }
        }

        // During non-runtime.
        if (!SceneManager.GetActiveScene().isLoaded)
        {
            // This check can be made aslong as the scene isn't loaded, probably.
            if (obj.gameObject.scene.name == null)
            {
                if (UnityEngine.Application.isPlaying)
                {
                    // If this warning ever appears during runtime, we need to take a look at it.
                    Debug.LogError("We found the object with an unsafe check.");
                }

                return true;
            }
        }

        return false;
    }

    #region String

    public static bool IsStringIndexWithinRichText(string text, int index)
    {
        int currIndex = index;
        while (currIndex > 0 && currIndex < text.Length)
        {
            if (text[currIndex] == '<')
            {
                return true;
            }

            currIndex++;
        }

        return false;
    }

    public static int FindIndexOutsideRichText(string text, int index)
    {
        int currIndex = index;

        if (!IsStringIndexWithinRichText(text, currIndex))
        {
            return currIndex;
        }

        // Assume we are already inside rich text because we check that above.
        while (true)
        {
            char currValue = text[currIndex];

            if (currIndex + 1 >= text.Length) // Outside of array.
            {
                return currIndex;
            }

            if (text[currIndex] == '/')
            {
                // Found ender - check if we can find a new beginner after it.
                while (text[currIndex] != '>')
                {
                    currIndex++;
                }

                if (text[currIndex + 1] != '<')
                {
                    // Found end!
                    return currIndex + 1;
                }
            }

            currIndex++;

        }
    }

    #endregion

    private static int GetIndexRound<T>(int index, List<T> list) where T : class
    {
        if (index < list.Count)
        {
            return index;
        }
        else
        {
            int roundIndex = index % list.Count;
            return roundIndex;
        }
    }

    private static int GetIndexRound(int index, int count)
    {
        if (index < count)
        {
            return index;
        }
        else
        {
            int roundIndex = index % count;
            return roundIndex;
        }
    }

    #region Coroutine

    public static void InvokeDelayed(System.Action action, int frameDelay)
    {
        IEnumerator ie = InvokeDelayedIE(action, frameDelay);
        Instance.StartCoroutine(ie);
    }

    public static void InvokeDelayed(System.Action action, float timeDelay)
    {
        IEnumerator ie = InvokeDelayedIE(action, timeDelay);
        Instance.StartCoroutine(ie);
    }

    private static IEnumerator InvokeDelayedIE(System.Action action, int frameDelay)
    {
        int currCount = 0;
        while (currCount < frameDelay)
        {
            currCount++;
            yield return new WaitForEndOfFrame();
        }

        if (action != null)
        {
            action.Invoke();
        }
    }

    private static IEnumerator InvokeDelayedIE(System.Action action, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        if (action != null)
        {
            action.Invoke();
        }
    }

    #endregion

    #region Color

    public static Color32 GetPlayerColor(int index)
    {
        return Instance.playerColorList[GetIndexRound(index, Instance.playerColorList.Count)];
    }

    public static string GetPlayerColorHex(int index)
    {
        return ColorUtility.ToHtmlStringRGB(GetPlayerColor(index));
    }

    #endregion

    #region Question Util

    public static string ToColoredNamesString(string s)
    {
        string newString = "";        
        string[] splitArray = s.Split(Instance.toSplitBy, System.StringSplitOptions.None);

        if (splitArray.Length == 0)
        {
            return s;
        }

        bool endsWithSplitValue = StringEndsWithAny(splitArray[splitArray.Length - 1], Instance.toSplitBy);

        for (int i = 0; i < splitArray.Length; i++)
        {
            // Make sure to only add the value in the end if it actually ends with it.
            string addValue = i < splitArray.Length - 1 || endsWithSplitValue ? " <b><color=#" + GetPlayerColorHex(i) + ">X</color></b> " : "";
            newString += splitArray[i] + addValue;
        }
        
        return newString;
    }

    public static string ToColoredNamesString(string s, List<System.Tuple<string, string>> playerList)
    {
        if (playerList == null || playerList.Count == 0)
        {
            const string errorMsg = "Invalid player list, it's empty or null.";
            ConfirmScreen.Create().Set(errorMsg, null, useCancel: false);
            Debug.LogError(errorMsg);
            return "";
        }

        string newString = "";
        string[] splitArray = s.Split(Instance.toSplitBy, System.StringSplitOptions.None);

        if (splitArray.Length == 0)
        {
            return s;
        }

        bool endsWithSplitValue = StringEndsWithAny(splitArray[splitArray.Length - 1], Instance.toSplitBy);
        List<System.Tuple<string, string>> validPlayerList = new List<System.Tuple<string, string>>();
        int playerIndex = 0;
        for (int i = 0; i < splitArray.Length; i++)
        {
            if (validPlayerList.Count == 0)
            {
                validPlayerList.AddRange(playerList); // List emptied -> refill it.
            }

            playerIndex = Random.Range(0, validPlayerList.Count);

            // Make sure to only add the value in the end if it actually ends with it.
            string addValue = i < splitArray.Length - 1 || endsWithSplitValue ? " <b><color=#" + validPlayerList[playerIndex].Item2 + ">" + validPlayerList[playerIndex].Item1 + "</color></b> " : "";
            newString += splitArray[i] + addValue;

            validPlayerList.RemoveAt(playerIndex); // Remmove the name used so it won't be used again at first.
        }

        return newString;
    }

    private static bool StringEndsWithAny(string s, string[] sArr)
    {
        for (int i = 0; i < sArr.Length; i++)
        {
            if (s.EndsWith(sArr[i]))
            {
                return true;
            }
        }

        return false;
    }

    private static bool StringBeginsWithAny(string s, string[] sArr)
    {
        for (int i = 0; i < sArr.Length; i++)
        {
            if (s.StartsWith(sArr[i]))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    [System.Serializable]
    public class UIDict : SerializableDictionaryBase<UIType, UIBase>
    {
    }
}
