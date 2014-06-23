using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour
{
	public float depthX, depthY;
	GameObject player;
	void Start ()
	{
		player = GameObject.Find("Player");
		if (player == null)
		{
			Debug.Log("player not found");
		}
	}
	
	void Update ()
	{
		if (player == null)
		{
			player = GameObject.Find("Player");
		}
		if (player != null)
		{
			transform.position = new Vector3(player.transform.position.x * depthX, player.transform.position.y * depthY);
		}
	}
}
