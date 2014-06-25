﻿using UnityEngine;
using System.Collections;

public class BackgroundParticleGenerator : MonoBehaviour
{
	public float rate;
	public int count; //how many particles do we spawn before destroying (0 = infinite)
	public float timeToLive; //how long do we live before destroying (0 = infinite)
	float timer;
	Transform topLeft, bottomRight;
	public GameObject[] particles; //specify the count of particles, no need to specify the gameobjects themselves
	public string resourcePath; //path + prefix, where the script tries to locate particles

	GameObject backgroungParticlesObj;

	void Start ()
	{
		timer = 0.0f;
		
		bottomRight = GameObject.Find(name + "/Top Left").transform;
		topLeft = GameObject.Find(name + "/Bottom Right").transform;
		backgroungParticlesObj = GameObject.Find("_BackgroungParticles");

		//particles = new GameObject[4];

		for (int i = 0; i < particles.Length; i++)
		{
			//particles[i] = new GameObject();
			particles[i] = Resources.Load<GameObject>(resourcePath + i); //"Particles/Background Particles/BGParticle"
		}
	}
	
	void Update ()
	{
		timer -= Time.deltaTime;
		if (timer <= 0.0f)
		{
			timer += rate;
			if (particles != null)
			{
				int i = 0;
				if (particles.Length > 1) //random particle if there's more of them
				{
					i = Random.Range(0, particles.Length);
				}
				//Debug.Log("rand " + i);

				if (particles[i] != null)
				{
					if (!topLeft || !bottomRight)
					{
						bottomRight = GameObject.Find(name + "/Top Left").transform;
						topLeft = GameObject.Find(name + "/Bottom Right").transform;
						/*Debug.Log("AFAGHNEDGS");
						Debug.Log("c: " + count);
						Debug.Log("ttl: " + timeToLive);
						Debug.Log("t: " + timer);
						return;*/
					}
					float x1 = topLeft.transform.position.x;
					float y1 = topLeft.transform.position.y;
					float x2 = bottomRight.transform.position.x;
					float y2 = bottomRight.transform.position.y;

					float width = x2 - x1;
					float height = y1 - y2;
					
					float posX = x1 + width * Random.value;
					float posY = y2 + height * Random.value;

					//Debug.Log("w" + width + "   h" + height);

					Vector3 position = new Vector3(posX, posY);
					GameObject temp = Instantiate(particles[i], position, Quaternion.identity) as GameObject;
					temp.transform.parent = backgroungParticlesObj.transform;
					temp.GetComponent<BackgroundParticle>().dir = Random.value * 360.0f;

					if (count > 0)
					{
						count--;
						if (count <= 0)
						{
							DestroyImmediate(this.gameObject);
							return;
						}
					}
				}
			}
		}

		

		if (timeToLive > 0.0f)
		{
			timeToLive -= Time.deltaTime;
			if (timeToLive <= 0.0f)
			{
				DestroyImmediate(this.gameObject);
				return;
			}
		}
	}
}
