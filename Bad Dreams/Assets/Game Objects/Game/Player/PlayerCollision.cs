using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private Player player;
	//Vector3 offsetFromPlatform;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
		if (!player.onGround)
		{
			//player.parentObject = null;
			transform.parent = null;
		}
		/*else
		{
			if (player.parentObject)
			{
				transform.position = player.parentObject.transform.position + player.offsetFromPlatform;
			}
		}*/
    }

    // Change to use tags such as "Enemy" & "Moving Platform"
    private void OnCollisionEnter2D(Collision2D other)
    {
		if (other == null)
		{
			Debug.Log("collision2d null");
		}
		if (other.gameObject == null)
		{
			Debug.Log("collision2d gameobject null");
		}
		if (player == null)
		{
			player = GetComponent<Player>();
			if (player == null)
				Debug.Log("player null");
		}

		//move to player terrain collision?
		if ((other.gameObject.tag == "Moving Environment" || other.gameObject.tag == "Moving and One Way") && player.onGround)
		{
			//player.parentObject = other.gameObject;
			//player.offsetFromPlatform = transform.position - player.parentObject.transform.position + new Vector3(0.0f,-0.005f,0.0f);
			//Debug.Log("offset " + player.offsetFromPlatform);
			transform.parent = other.gameObject.transform;
		}
    }

	/*void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.name == "Lethal Spikes")
		{
			if (player.rigidbody2D.velocity.y < -6.0f)
				player.GetComponent<HitAnimation>().ActivateAnimation();
		}
	}*/
}
