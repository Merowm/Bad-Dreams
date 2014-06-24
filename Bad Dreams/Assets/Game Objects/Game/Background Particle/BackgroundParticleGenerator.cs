using UnityEngine;
using System.Collections;

public class BackgroundParticleGenerator : MonoBehaviour
{
	public float rate;
	float timer;
	Transform topLeft, bottomRight;
	GameObject[] particles;

	void Start ()
	{
		timer = 0.0f;

		topLeft = GameObject.Find("Top Left").transform;
		bottomRight = GameObject.Find("Bottom Right").transform;

		particles = new GameObject[4];

		for (int i = 0; i < particles.Length; i++)
		{
			particles[i] = new GameObject();
			particles[i] = Resources.Load<GameObject>("Particles/Background Particles/BGParticle" + i);
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
				int i = Random.Range(0, particles.Length);
				//Debug.Log("rand " + i);

				if (particles[i] != null)
				{
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
					temp.GetComponent<BackgroundParticle>().dir = Random.value * 360.0f;
				}
			}
		}
	}
}
