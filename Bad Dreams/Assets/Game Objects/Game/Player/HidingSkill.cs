using UnityEngine;
using System.Collections;

/// <summary>
/// Performs the player's 
/// hiding skill.
/// </summary>
public class HidingSkill : MonoBehaviour
{
    public bool IsHiding { get; set; }
    public bool OverCoverObject { get; set; }
    public GameObject CoverObject { get; set; }

    private GameObject uiHidingEffect;
    private TweenAlpha tweenAlpha;

    private SpriteRenderer spriteRenderer;
        
    private void Start()
    {
        spriteRenderer = transform.FindChild("Animator").GetComponent<SpriteRenderer>();
        uiHidingEffect = GameObject.Find("UI Hiding Effect");
        tweenAlpha = uiHidingEffect.GetComponent<TweenAlpha>();
        uiHidingEffect.SetActive(false);
    }

    public void Hide(GameObject cover)
    {
        IsHiding = true;
        SwapLayerTo("Player Background");
        Physics2D.IgnoreLayerCollision(9, 10, true);
        CoverObject = cover;
        uiHidingEffect.SetActive(true);
        tweenAlpha.PlayForward();
    }

    public void Unhide()
    {
        tweenAlpha.PlayReverse();
        IsHiding = false;
        SwapLayerTo("Player Foreground");
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private void SwapLayerTo(string layer)
    {
        spriteRenderer.sortingLayerName = layer;
    }
}

