using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	public float floatingSpeed, floatingAmount;

	bool activated;
	float radians;

	Vector3 startPosition;

	void Start ()
	{
		startPosition = transform.position;
		activated = false;
		radians = 0.0f;
	}
	
	void Update ()
	{
		radians += Time.deltaTime * floatingSpeed;
		if (radians >= 2.0f * Mathf.PI)
		{
			radians -= 2.0f * Mathf.PI;
		}
		transform.position = startPosition + new Vector3(0.0f, Mathf.Sin(radians) * floatingAmount);

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
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

			SetCurrentCheckpoint();
			activated = true;
		}
	}

	void DeActivate()
	{
		if (activated)
		{
			Debug.Log("Deactivated checkpoint");
			GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
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
