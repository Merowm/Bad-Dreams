using UnityEngine;
using System.Collections;

public class DropCounter : MonoBehaviour
{
    private int dropCount;
    private UILabel uiLabel;

    private TweenScale spriteScaleTween;
    private TweenScale textScaleTween;

    private void Start()
    {
        dropCount = 0;
        GameObject dropCounterText = GameObject.Find("Drop Counter Text");
        uiLabel = dropCounterText.GetComponentInChildren<UILabel>();
        textScaleTween = dropCounterText.GetComponent<TweenScale>();
        spriteScaleTween = GetComponent<TweenScale>();
    }

    public int DropCount
    {
        get { return dropCount; }
        set
        {
            PlayTween();
            dropCount = Mathf.Clamp(value, 0, int.MaxValue);
            uiLabel.text = dropCount.ToString();
        }
    }

    private void PlayTween()
    {
        if (spriteScaleTween == null ||
            textScaleTween == null)
        {
            return;
        }

        spriteScaleTween.ResetToBeginning();
        textScaleTween.ResetToBeginning();

        spriteScaleTween.enabled = true;
        textScaleTween.enabled = true;

        spriteScaleTween.PlayForward();
        textScaleTween.PlayForward();
    }
}
