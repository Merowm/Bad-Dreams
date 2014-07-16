using UnityEngine;
using System.Collections;

public class PlayerParticle : MonoBehaviour
{
	public bool randomDirection;
	public float spinSpeed;

	float dir;
	void Start ()
	{
		dir = 0.0f;
		if (randomDirection)
		{
			dir = Random.value * 360.0f;
			
		}
		transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), dir);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), spinSpeed);
		//dir += Time.deltaTime * spinSpeed;
	}
}
