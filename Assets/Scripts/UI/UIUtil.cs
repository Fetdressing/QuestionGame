using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil : MonoBehaviour
{
    private static UIUtil instance;
    

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


}
