using UnityEngine;
using System.Collections;

public class SnowParticle : MonoBehaviour
{
	Particle part;
	float curvePos, curveSpeed;
	public AnimationCurve xCurve;

	void Start ()
	{
		part = GetComponent<Particle>();

		part.velocity = new Vector3(Random.value * 1.0f - 0.5f, -Random.value);
		part.velocity.Normalize();

		curvePos = 0.0f + Random.value * 0.3f;
		curveSpeed = Random.value * 0.05f + 0.15f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		curvePos += curveSpeed * Time.deltaTime;
		if (curvePos >= 1.0f)
		{
			curvePos = 0.0f;
		}

		part.velocity += new Vector3(0.6f, 0.0f) * xCurve.Evaluate(curvePos) * Time.deltaTime;

	}
}
