using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers the availability of
/// the player's stealth skill.
/// </summary>
public class StealthUsableArea : MonoBehaviour
{
    private string targetObject;
    private HidingSkill player;

    private void Start()
    {
        targetObject = "Player";
        player = GameObject.Find("Player").GetComponent<HidingSkill>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            player.CoverObject = gameObject;
            player.HidingPossible = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            if (player.CoverObject.tag != "Flower Cover Skill" ||
                !player.IsHiding)
            {
                if (!player.OverCoverObject)
                    player.HidingPossible = true;

                player.CoverObject = gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            player.HidingPossible = false;
            if (player.IsHiding)
                player.Unhide();
        }
    }
}
