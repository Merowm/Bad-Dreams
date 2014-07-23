using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	bool activated;

	//SpriteRenderer sprite, runes;
	Animator atorLamp, atorRunes;
	ParticleGenerator pGen;

	public GameObject pGenObj, pGenPrefab;
	Vector3 pGenPos;

	void Start ()
	{
		pGenPos = new Vector3(0.05812454f, 0.362566f, 0.0f);
		pGenPrefab = Resources.Load<GameObject>("Checkpoint/Checkpoint Particle Generator");
		pGen = null;
		
		pGenObj = Instantiate(pGenPrefab, transform.position+pGenPos, Quaternion.identity) as GameObject;
		pGenObj.transform.parent = transform;
		pGenObj.name = "Checkpoint Particle Generator " + transform.position.x;

		pGen = pGenObj.GetComponent<ParticleGenerator>();


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
			//pGen = transform.Find("Checkpoint Particle Generator").gameObject.GetComponent<ParticleGenerator>(); //???
		}
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
			//Debug.Log("ZECPOENT");

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
