using UnityEngine;
using System.Collections;

public class FallingAnimation : MonoBehaviour
{
	public float timeBeforeTransition;		//time between getting hit and transition beginning
	public bool active;

	float preTransitionTimer;
	Player player;
	void Start ()
	{
		player = GetComponent<Player>();
	}
	
	void Update ()
	{
		if (active)
		{
			preTransitionTimer += Time.deltaTime;
			if (preTransitionTimer >= timeBeforeTransition)
			{
				StartTransition();
			}
		}
	}



	public void ActivateAnimation() //call this when hit with the trigger
	{
		//Debug.Log("ActivateAnimation");
		if (!active)
		{
			//Debug.Log("ActivateAnimation success");
			preTransitionTimer = 0.0f;
			active = true;
			player.allowCameraFollowing = false;
		}
	}

	public void ResetAnimation()
	{
		//Debug.Log("ResetAnimation");
		if (active)
		{
			active = false;
			preTransitionTimer = 0.0f;
			player.allowCameraFollowing = true;
		}
	}


	void StartTransition()
	{
		//Debug.Log("start transition from falling");
		GameplayStateManager.SwitchTo(GameplayState.GameOver);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.name == "Falling Animation Trigger")
		{
			ActivateAnimation();
		}
	}
}
