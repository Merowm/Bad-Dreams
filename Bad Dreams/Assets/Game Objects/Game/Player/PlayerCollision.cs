using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
		if (player == null)
		{
			Debug.Log("player null start");
		}
    }

    private void Update()
    {
        if (!player.onGround)
            transform.parent = null;

		if (player == null)
		{
			Debug.Log("player null update");
		}
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



        if (other.gameObject.name == "Dog")
            GameplayStateManager.SwitchTo(GameplayState.GameOver);

		if ((other.gameObject.tag == "Moving Environment" || other.gameObject.tag == "Moving and One Way") && player.onGround)
            transform.parent = other.gameObject.transform;
    }
}
