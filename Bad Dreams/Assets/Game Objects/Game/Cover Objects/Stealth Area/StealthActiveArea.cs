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
		targetObject = "Player Flower Collider";
        player = GameObject.Find("Player").GetComponent<HidingSkill>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log("enter " + other.name);
        if (other.name == targetObject && Input.GetButton("Hide"))
        {
            player.Hide(gameObject);
        }
        else if (other.name == targetObject)
        {
            if (!player.IsHiding)
                player.OverCoverObject = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
		Debug.Log("stay " + other.name);
        if (other.name == targetObject)
        {
            if (!player.IsHiding)
                player.OverCoverObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
		Debug.Log("exit " + other.name);
        if (other.name == targetObject)
        {
            if (player.IsHiding)
            {
                player.Unhide();
            }

            player.OverCoverObject = false;
        }
    }
}
