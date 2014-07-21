using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	bool activated;

	//SpriteRenderer sprite, runes;
	Animator atorLamp, atorRunes;
	ParticleGenerator pGen;

	public GameObject pGenObj;

	void Start ()
	{
		pGen = null;
		
		//pGen = pGenObj.GetComponent<ParticleGenerator>();
		//Debug.Log("checpoints are so stupid they don't even know their names: " + name);

		activated = false;
	}
	
	void Update ()
	{

	}

	void AcquireThings()
	{
		if (!atorLamp)
			atorLamp = GameObject.Find(gameObject.name + "/Lamp").GetComponent<Animator>();
		
		if (!atorRunes)
			atorRunes = GameObject.Find(gameObject.name + "/Runes").GetComponent<Animator>();


		if (!pGen)
		{
			//Transform pGenT = this.transform.Find("Checkpoint Particle Generator");
			//pGen = pGenT.gameObject.GetComponent<ParticleGenerator>();

			Debug.Log(gameObject.name + "/Checkpoint_Particle_Generator");
			pGen = GameObject.Find(gameObject.name + "/Checkpoint_Particle_Generator").GetComponent<ParticleGenerator>();

		}
		//pGen = GameObject.Find(gameObject.name + "/Checkpoint Particle Generator").GetComponent<ParticleGenerator>();
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
		AcquireThings();
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
		AcquireThings();
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
