using UnityEngine;
using System.Collections;

/// <summary>
/// Disables player stealth on exit.
/// </summary>
public class StealthActiveArea : MonoBehaviour
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
            player.HidingPossible = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == targetObject)
        {
            HidingSkill player = other.GetComponent<HidingSkill>();

            if (player.IsHiding)
            {
                player.SwapLayer();
                player.IsHiding = false;
            }

            player.HidingPossible = true;
        }
    }
}
