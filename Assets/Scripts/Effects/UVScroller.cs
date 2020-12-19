using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UVScroller : UIBase
{
    /// <summary>
    /// The raw image to scroll.
    /// </summary>
    public RawImage image;

    /// <summary>
    /// The normal scrolling.
    /// </summary>
    public Vector2 linearScrolling = Vector2.zero;

    /// <summary>
    /// Swaying frequency.
    /// </summary>
    [Range(0.1f, 50)]
    public float swayingFrequency = 1;

    /// <summary>
    /// Swaying scrolling. How the UV should sway.
    /// </summary>
    public Vector2 swayingScrolling = Vector2.zero;

    private IEnumerator actionIE;

    private void OnEnable()
    {
        if (actionIE != null)
        {
            StopCoroutine(actionIE);
        }

        actionIE = Scroll();
        StartCoroutine(actionIE);
    }

    private void OnDisable()
    {
        if (actionIE != null)
        {
            StopCoroutine(actionIE);
        }
    }

    /// <summary>
    /// Perform animation.
    /// </summary>
    /// <returns>Returns back to update every frame.</returns>
    private IEnumerator Scroll()
    {
        while (this != null)
        {
            if (this.gameObject.activeInHierarchy == false)
            {
                break;
            }

            Vector2 currPos = image.uvRect.position;
            Vector2 newPos = currPos;

            newPos += new Vector2(linearScrolling.x * Time.deltaTime, linearScrolling.y * Time.deltaTime);

            const float swayingAmplitudeMult = 0.001f;
            newPos += new Vector2(Mathf.Cos(Time.time * swayingFrequency) * swayingScrolling.x * swayingAmplitudeMult, Mathf.Cos(Time.time * swayingFrequency) * swayingScrolling.y * swayingAmplitudeMult);

            image.uvRect = new Rect(newPos, image.uvRect.size);

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// On validate.
    /// </summary>
    private void OnValidate()
    {
        this.image = this.GetComponent<RawImage>();
    }
}
