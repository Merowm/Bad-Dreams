using UnityEngine;
using System.Collections;

public class ParticleGenerator : MonoBehaviour
{
	public float rate;				//generation rate
	public int count;				//how many particles do we spawn before destroying (0 = infinite)
	public float timeToLive;		//how long do we live before destroying (0 = infinite)
	public GameObject[] particles;	//specify the count of particles, no need to specify the gameobjects themselves
	public string resourcePath;		//path + prefix, where the script tries to locate particles (ex. "Particles/Particle" seeks prefabs named Particle0, Particle1, ...)
	public string parentName;		//all particles are created inside this gameobject
	public bool dontDestroy;		//don't destroy, when count == 0 || ttl == 0

	int startingCount;
	float timer;
	Transform topLeft, bottomRight;
	GameObject backgroundParticlesObj;

	void Start ()
	{
		timer = 0.0f;
		startingCount = count;

		bottomRight = GameObject.Find(name + "/Top Left").transform;
		topLeft = GameObject.Find(name + "/Bottom Right").transform;
		backgroundParticlesObj = GameObject.Find(parentName);
		
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i] = Resources.Load<GameObject>(resourcePath + i);
			if (!particles[i])
			{
				Debug.Log("null particle");
			}
		}
	}

	public void Trigger()
	{
		//Debug.Log(name + " Trigger()");
		count = startingCount;
		timer = 0.0f;
	}
	
	void Update ()
	{
		if (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			if (timer < 0.0f)
				timer = 0.0f;
		}

		if (timer <= 0.0f && (count > 0 || startingCount == 0)) //if there are particles left to generate
		{
			timer = rate;
			if (particles != null)
			{
				int i = 0;
				if (particles.Length > 1) //random particle if there's more of them
				{
					i = Random.Range(0, particles.Length);
				}
				if (particles[i] != null)
				{
					if (!topLeft || !bottomRight)
					{
						//try reacquire if not found
						bottomRight = GameObject.Find(name + "/Top Left").transform;
						topLeft = GameObject.Find(name + "/Bottom Right").transform;
					}
					float x1 = topLeft.transform.position.x;
					float y1 = topLeft.transform.position.y;
					float x2 = bottomRight.transform.position.x;
					float y2 = bottomRight.transform.position.y;

					float width = x2 - x1;
					float height = y1 - y2;
					
					float posX = x1 + width * Random.value;
					float posY = y2 + height * Random.value;

					Vector3 position = new Vector3(posX, posY);
					GameObject temp = Instantiate(particles[i], position, Quaternion.identity) as GameObject;
					temp.transform.parent = backgroundParticlesObj.transform;
					temp.GetComponent<Particle>().dir = Random.value * 360.0f; //Todo: move this to background particle start

					if (count > 0)
					{
						count--;
						if (count <= 0 && !dontDestroy)
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
			if (timeToLive <= 0.0f && !dontDestroy)
			{
				DestroyImmediate(this.gameObject);
				return;
			}
		}
	}
}
