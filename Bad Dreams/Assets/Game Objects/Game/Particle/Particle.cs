using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour
{
	public float speed, dir, addDir, ttl;


	float rad; //degrees in radians
	float ttlBegin; //time to live value for referencing color phases
	Vector3 direction;

	public Gradient colorIn;
	public AnimationCurve addDirMultiplyCurve;

	public Vector3 velocity, addVelocity;

	public bool randomAddVelocityEnabled;

	public float randomAddVelocityMultiplier;

	Vector3 randomAddVelocity;
	SpriteRenderer spr;

	void Start ()
	{
		spr = GetComponent<SpriteRenderer>();
		ttlBegin = ttl;

		if (randomAddVelocityEnabled)
		{
			addVelocity = new Vector3(Random.value * 2.0f - 1.0f,Random.value * 2.0f - 1.0f,0.0f);
			addVelocity.Normalize();
			addVelocity *= randomAddVelocityMultiplier;
		}


	}
	
	void Update()
	{
		float pos = 1.0f - ttl / ttlBegin; //current position in curves

		dir += Time.deltaTime * addDir * addDirMultiplyCurve.Evaluate(pos);
		spr.color = colorIn.Evaluate(1.0f - ttl / ttlBegin);
		
		if (ttl > 0.0f)
		{
			ttl -= Time.deltaTime;
			if (ttl < 0.0f)
			{
				ttl = 0.0f;
				Destroy(this.gameObject);
			}
		}
	

		//->
		rad = DegToRad(dir);
		float xx = Mathf.Cos(rad);
		float yy = Mathf.Sin(rad);
		direction = new Vector3(xx, yy);
		transform.position += direction * speed * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		velocity += addVelocity * Time.deltaTime;
		velocity += randomAddVelocity * Time.deltaTime;
	}

	float DegToRad(float deg)
	{
		return (deg / 360.0f) * Mathf.PI * 2.0f;
	}
}
