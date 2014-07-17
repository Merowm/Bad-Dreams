using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	bool activated;

	//SpriteRenderer sprite, runes;
	Animator atorLamp, atorRunes;
	ParticleGenerator pGen;

	void Start ()
	{
		atorLamp = GameObject.Find(name + "Lamp").GetComponent<Animator>();
		atorRunes = GameObject.Find(name + "Runes").GetComponent<Animator>();
		pGen = GameObject.Find(name + "Checkpoint Particle Generator").GetComponent<ParticleGenerator>();
		
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
			pGen.Trigger();

			SetCurrentCheckpoint();
			activated = true;
		}
	}

	public void DeActivate()
	{
		if (activated)
		{
			//Debug.Log("Deactivated checkpoint");
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
