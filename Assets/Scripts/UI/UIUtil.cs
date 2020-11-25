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

    [System.Serializable]
    public class UIDict : SerializableDictionaryBase<UIType, UIBase>
    {
    }
}
