using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers the availability of
/// the player's stealth skill.
/// </summary>
public class StealthUsableArea : MonoBehaviour
{
    private string targetObject;

    private void Start()
    {
        targetObject = "Player";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            HidingSkill player = other.GetComponent<HidingSkill>();
            player.CoverObject = gameObject;
            player.HidingPossible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            HidingSkill player = other.GetComponent<HidingSkill>();
            player.HidingPossible = false;
        }
    }
}
