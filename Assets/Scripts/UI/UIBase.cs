using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    protected RectTransform graphicBase;

    protected virtual void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    protected virtual void OnInit()
    {

    }

    private void Awake()
    {
        OnInit();
    }
}
