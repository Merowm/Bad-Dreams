using UnityEngine;
using System.Collections;

public class Butterfly : MonoBehaviour
{
	float flapper;				//timer
	float flapper2;				//another timer

	float flapperMul, flapperMul2;

	float flapFreq;				//flapping frequency

	Vector3 flapVelocity;
	bool up;
	Vector3 velocity, velocity2;

	public Sprite[] sprites;
	int frame;
	SpriteRenderer spr;
	Particle particle;

	float turnTimer, turnFreq;
	bool mirror;

	void Start ()
	{
		spr = GetComponent<SpriteRenderer>();
		particle = GetComponent<Particle>();
		flapper = 0.0f;
		flapper2 = 0.0f;

		flapperMul = 1.0f * (0.85f + Random.value * 0.15f);
		flapperMul2 = 1.0f * (0.85f + Random.value * 0.15f);

		flapFreq = 0.09f * (0.85f + Random.value * 0.15f);
		up = true;
		velocity = Vector3.zero;
		flapVelocity = new Vector3(0.0f, 0.7f);

		frame = 0;

		turnTimer = 0.0f;
		turnFreq = 3.4f * (0.85f + Random.value * 0.15f);
		mirror = false;
	}
	
	void Update ()
	{
		//flapping pattern
		flapper += Time.deltaTime * flapperMul;
		if (flapper >= flapFreq)
		{
			flapper = 0.0f;
			if (up)
			{
				velocity = flapVelocity;
			}
			else
			{
				velocity = -flapVelocity;
			}
			up = !up;
			ChangeFrame();

			velocity += new Vector3((Random.value * 0.5f) - 0.25f, 0.0f);
		}

		//circular pattern
		flapper2 += Time.deltaTime * 4.0f * flapperMul2;
		if (flapper2 >= Mathf.PI * 2.0f)
		{
			flapper2 = 0.0f;
		}
		float speed = 0.5f;
		velocity2 = new Vector3(Mathf.Sin(flapper2) * 0.25f, Mathf.Cos(flapper2)) * speed;

		//turning around
		turnTimer += Time.deltaTime;

		if (turnTimer >= turnFreq)
		{
			//particle.speed *= -1.0f;
			particle.dir += 180;
			turnTimer = 0.0f;
			mirror = !mirror;
		}


		//animation
		spr.sprite = sprites[frame];

		//velocity
		transform.position += velocity * Time.deltaTime;
		transform.position += velocity2 * Time.deltaTime;

		//mirror
		if (mirror)
			transform.localScale = new Vector3(-1.0f, transform.localScale.y, 1.0f);
		else
			transform.localScale = new Vector3(1.0f, transform.localScale.y, 1.0f);

	}

	void ChangeFrame()
	{
		if (frame < sprites.Length - 1)
		{
			frame++;
		}
		else
		{
			frame = 0;
		}
	}
}
