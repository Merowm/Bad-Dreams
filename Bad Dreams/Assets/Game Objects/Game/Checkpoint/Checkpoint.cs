using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	bool activated;

	//SpriteRenderer sprite, runes;
	Animator atorLamp, atorRunes;

	void Start ()
	{
		atorLamp = GameObject.Find("Lamp").GetComponent<Animator>();
		atorRunes = GameObject.Find("Runes").GetComponent<Animator>();
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
			atorLamp.SetTrigger("activate");
			atorRunes.SetTrigger("activate");
			SetCurrentCheckpoint();
			activated = true;
		}
	}

	public void DeActivate()
	{
		if (activated)
		{
			Debug.Log("Deactivated checkpoint");
			atorLamp.SetTrigger("deActivate");
			atorRunes.SetTrigger("deActivate");
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
