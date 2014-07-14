using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	bool activated;

	SpriteRenderer sprite, runes;

	void Start ()
	{
		sprite = GameObject.Find("Post").GetComponentInChildren<SpriteRenderer>();
		runes = GameObject.Find("Runes").GetComponentInChildren<SpriteRenderer>();
		activated = false;
	}
	
	void Update ()
	{

	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c)
		{
			if (c.name == "Player")
			{
				Activate();
			}
		}
	}

	void Activate()
	{
		if (!activated)
		{
			Debug.Log("ZECPOENT");
			//GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

			SetCurrentCheckpoint();
			activated = true;
		}
	}

	void DeActivate()
	{
		if (activated)
		{
			Debug.Log("Deactivated checkpoint");
			//GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
			activated = false;
		}
	}

	void SetCurrentCheckpoint()
	{
		GameObject player = GameObject.Find("Player");
		if (player)
		{
			player.GetComponent<Player>().SetCurrentCheckpoint(this.gameObject);
		}
	}
}
