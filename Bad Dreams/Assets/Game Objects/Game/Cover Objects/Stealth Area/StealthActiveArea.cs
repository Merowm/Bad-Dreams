using UnityEngine;
using System.Collections;

/// <summary>
/// Disables player stealth on exit.
/// </summary>
public class StealthActiveArea : MonoBehaviour
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
        if (other.name == targetObject && Input.GetButton("Hide"))
        {
            player.Hide(player.CoverObject);
            player.HidingPossible = false;
        }
        else if (other.name == targetObject)
        {
            if (!player.IsHiding)
                player.OverCoverObject = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == targetObject)
            player.HidingPossible = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            if (player.IsHiding)
            {
                player.Unhide();
            }

            player.OverCoverObject = false;
            player.HidingPossible = false;
        }
    }
}
