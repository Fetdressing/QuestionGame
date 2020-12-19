using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

[RequireComponent(typeof(UnityEngine.UI.MaskableGraphic))]
public class EnableAlphaLerp : UIBase
{
    [SerializeField]
    private MaskableGraphic[] maskableGraphics;

    [SerializeField]
    [HideInInspector]
    private Color[] startColors;

    protected override void OnEditorUpdate()
    {
        base.OnEditorUpdate();
        maskableGraphics = GetComponentsInChildren<MaskableGraphic>();
        startColors = new Color[maskableGraphics.Length];

        for (int i = 0; i < maskableGraphics.Length; i++)
        {
            startColors[i] = maskableGraphics[i].color;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < maskableGraphics.Length; i++)
        {
            maskableGraphics[i].DOKill();
            maskableGraphics[i].color = Color.clear;
            maskableGraphics[i].DOColor(startColors[i], 0.3f);
        }
    }
}
