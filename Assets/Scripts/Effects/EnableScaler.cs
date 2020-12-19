using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class EnableScaler : UIBase
{
    [SerializeField]
    private AnimationCurve scaleCurve;

    [SerializeField]
    private Vector3 startScale;

    private IEnumerator actionIE;

    protected override void OnEditorUpdate()
    {
        base.OnEditorUpdate();
        startScale = this.transform.localScale;
    }

    private void OnEnable()
    {
        if (actionIE != null)
        {
            StopCoroutine(actionIE);
            actionIE = null;
        }

        actionIE = DoScale();
        StartCoroutine(actionIE);
    }

    private IEnumerator DoScale()
    {
        this.transform.localScale = Vector3.zero;
        float startTime = Time.time;
        float localEndTime = scaleCurve.keys[scaleCurve.length - 1].time;
        while (Time.time - startTime < localEndTime)
        {
            float currScale = scaleCurve.Evaluate(Time.time - startTime);
            this.transform.localScale = new Vector3(startScale.x * currScale, startScale.y * currScale, startScale.z * currScale);
            yield return new WaitForEndOfFrame();
        }        
    }
}
