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

	SpriteRenderer spr;

	void Start ()
	{
		spr = GetComponent<SpriteRenderer>();
		ttlBegin = ttl;
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
	}

	float DegToRad(float deg)
	{
		return (deg / 360.0f) * Mathf.PI * 2.0f;
	}
}
