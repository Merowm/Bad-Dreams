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
		atorLamp = GameObject.Find(gameObject.name + "/Lamp").GetComponent<Animator>();
		atorRunes = GameObject.Find(gameObject.name + "/Runes").GetComponent<Animator>();


		Transform pGenT = this.transform.FindChild("Checkpoint Particle Generator");

		pGen = pGenT.gameObject.GetComponent<ParticleGenerator>();
		//pGen = GameObject.Find(gameObject.name + "/Checkpoint Particle Generator").GetComponent<ParticleGenerator>();


		//Debug.Log("checpoints are so stupid they don't even know their names: " + name);

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
