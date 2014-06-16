using UnityEngine;
using System.Collections;

/// <summary>
/// Performs the player's 
/// hiding skill.
/// </summary>
public class HidingSkill : MonoBehaviour
{
    public bool IsHiding { get; set; }
    public bool HidingPossible { get; set; }
    public bool OverCoverObject { get; set; }
    public GameObject CoverObject { get; set; }

    private SpriteRenderer spriteRenderer;
        
    private void Start()
    {
        spriteRenderer = transform.FindChild("Animator").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Hide") &&
            HidingPossible &&
            !IsHiding)
        {
            Hide(CoverObject);
        }
    }

    public void Hide(GameObject cover)
    {
        IsHiding = true;
        HidingPossible = false;
        SwapLayerTo("Player Background");
        Physics2D.IgnoreLayerCollision(9, 10, true);
        transform.position = new Vector3(
            cover.transform.position.x,
            transform.position.y,
            cover.transform.position.z);

        CoverObject = cover;
        // Play animation?
        // Play sound?
        // Play particle effect?
    }

    public void Unhide()
    {
        IsHiding = false;
        HidingPossible = true;
        SwapLayerTo("Player Foreground");
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private void SwapLayerTo(string layer)
    {
        spriteRenderer.sortingLayerName = layer;
    }
}

