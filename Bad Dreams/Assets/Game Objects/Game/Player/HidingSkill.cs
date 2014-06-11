using UnityEngine;
using System.Collections;


/// <summary>
/// Swaps the player's sorting layer
/// for the stealth skill.
/// </summary>
public class HidingSkill : MonoBehaviour
{
    public bool IsHiding { get; set; }
    public bool HidingPossible { get; set; }
    public Transform CoverObject { get; set; }

    private SpriteRenderer spriteRenderer;
        
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            IsHiding = true;
            HidingPossible = false;

            SwapLayer();
            MovePlayerToCover();
            // Play animation?
            // Play sound?
            // Play particle effect?
        }
    }

    public void SwapLayer()
    {
        if (spriteRenderer.sortingLayerName == "Player Foreground")
            spriteRenderer.sortingLayerName = "Player Background";
        else
            spriteRenderer.sortingLayerName = "Player Foreground";
    }

    private void MovePlayerToCover()
    {
        transform.position = CoverObject.position;
    }
}

