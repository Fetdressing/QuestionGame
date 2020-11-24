using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RotaryHeart.Lib.SerializableDictionary;
using RotaryHeart;


public class UIUtil : MonoBehaviour
{
    private static UIUtil instance;

    [SerializeField]
    private UIDict uiDict;

    public enum UIType
    {
        ConfirmScreen
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

    [System.Serializable]
    public class UIDict : SerializableDictionaryBase<UIType, UIBase>
    {
    }
}
