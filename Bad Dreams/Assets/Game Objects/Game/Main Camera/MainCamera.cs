using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
	GameObject player;
	Transform minT, maxT;

	Vector3 min, max;
	void Update()
	{
		if (player == null)
		{
			player = GameObject.Find("Player");
		}

		minT = GameObject.Find("Camera Limit Min").transform;
		maxT = GameObject.Find("Camera Limit Max").transform;

		min = minT.position;
		max = maxT.position;
	}

	void LateUpdate()
	{
		/*if (player != null)
		{
			Vector3 pos = transform.position;
			Vector3 newPos = new Vector3(Mathf.Clamp(pos.x, min.x, max.x), Mathf.Clamp(pos.y, min.y, max.y), -10.0f);

			transform.position = newPos;
		}*/
	}
}
