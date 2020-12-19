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

    public enum UIType
    {
        ConfirmScreen,
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

    public static void InvokeDelayed(System.Action action, int frameDelay)
    {
        IEnumerator ie = InvokeDelayedIE(action, frameDelay);
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

    [System.Serializable]
    public class UIDict : SerializableDictionaryBase<UIType, UIBase>
    {
    }
}
