using UnityEngine;
using System.Collections;

public class StaminaParticle : MonoBehaviour
{

	Particle part;

	void Start()
	{
		part = GetComponent<Particle>();

		float separation = 0.25f;

		part.velocity = new Vector3(Random.value * separation - (separation / 2), Random.value * separation - (separation / 2));
		//part.velocity.Normalize();

		Animator ator = GetComponent<Animator>();
		//ator.StartPlayback();
		//ator.playbackTime = Random.value * 0.5f;

		ator.speed = 1.0f - Random.value * 0.33f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
