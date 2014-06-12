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
    public GameObject CoverObject { get; set; }

    private SpriteRenderer spriteRenderer;
        
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        HandleInput();
        Debug.Log("IsHiding: " + IsHiding);
        Debug.Log("Hiding possible: " + HidingPossible);
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
        SwapLayer();
        transform.position = cover.transform.position;
        // Play animation?
        // Play sound?
        // Play particle effect?
    }

    public void Unhide()
    {
        IsHiding = false;
        HidingPossible = true;
        SwapLayer();
    }

    private void SwapLayer()
    {
        if (spriteRenderer.sortingLayerName == "Player Foreground")
            spriteRenderer.sortingLayerName = "Player Background";
        else
            spriteRenderer.sortingLayerName = "Player Foreground";
    }
}

