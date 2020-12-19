using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGlow : UIBase
{
    /// <summary>
    /// Rotating speed of the effect.
    /// </summary>
    public float RotatingMultiplier = 1;

    /// <summary>
    /// Scale multiplier for how much it should change in scale.
    /// </summary>
    public float ScaleMultiplier = 0f;

    /// <summary>
    /// How fast it should scale.
    /// </summary>
    public float ScaleSpeedMultiplier = 1f;

    /// <summary>
    /// Start scale.
    /// </summary>
    [HideInInspector]
    [SerializeField]
    private Vector3 startScale = Vector3.one;

    private IEnumerator actionIE;

    private void OnEnable()
    {
        if (actionIE != null)
        {
            StopCoroutine(actionIE);
        }

        actionIE = Glow();
        StartCoroutine(actionIE);
    }

    private void OnDisable()
    {
        if (actionIE != null)
        {
            StopCoroutine(actionIE);
        }
    }

#if UNITY_EDITOR

    protected override void OnEditorUpdate()
    {
        base.OnEditorUpdate();
        this.startScale = this.transform.localScale;
    }

#endif

    /// <summary>
    /// Perform glow animation.
    /// </summary>
    /// <returns>Returns back to update every frame.</returns>
    private IEnumerator Glow()
    {
        float rotateSpeed = 5;

        while (this != null)
        {
            if (this.gameObject.activeInHierarchy == false)
            {
                break;
            }

            if (System.Math.Abs(RotatingMultiplier) > 0.001f)
            {
                this.transform.Rotate(new Vector3(0, 0, rotateSpeed * RotatingMultiplier * Time.deltaTime));
            }

            if (ScaleMultiplier > 0.001f)
            {
                this.transform.localScale = startScale * (1 + Mathf.PingPong(Time.time * ScaleSpeedMultiplier, ScaleMultiplier));
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
