using UnityEngine;
using System.Collections;

public class HitAnimation : MonoBehaviour
{
	public float timeBeforeTransition;		//time between getting hit and transition beginning
	public bool active;

	float preTransitionTimer;


	GameObject hitAnimation;
	GameObject hitAnimationPrefab;
	Player player;

	void Start ()
	{
		player = GetComponent<Player>();
		hitAnimation = Resources.Load<GameObject>("Player/Death");
		ResetAnimation();
	}

	void Update ()
	{
		GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = "preTransitionTimer " + preTransitionTimer;
		if (active)
		{
			preTransitionTimer += Time.deltaTime;
			if (preTransitionTimer >= timeBeforeTransition)
			{
				StartTransition();
				//ResetAnimation();
			}
		}
	}

	public void ActivateAnimation() //call this when get hit
	{
		Debug.Log("ActivateAnimation...");
		if (!active)
		{
			Debug.Log("ActivateAnimation success");
			active = true;
			//Physics2D.IgnoreLayerCollision(9, 10, true);
			player.Kill();
			//rigidbody2D.isKinematic = true;
			CreateParticles();
		}
	}

	void StartTransition()
	{
		Debug.Log("start transition");
		GameplayStateManager.SwitchTo(GameplayState.GameOver);
		//Transition transition = GameObject.Find("Transition").GetComponent<Transition>();
		//transition.PlayForward();
		//transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadLastCheckpoint"));
	}

	public void ResetAnimation()
	{
		if (active)
		{
			active = false;
			preTransitionTimer = 0.0f;
			//rigidbody2D.isKinematic = false;
			player.Resurrect();
			DeleteDeathPrefab();
			//DisableLayerCollision(false);
			//Physics2D.IgnoreLayerCollision(9, 10, false);
		}
	}

	void DisableLayerCollision(bool val)
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), val);
	}

	void CreateParticles()
	{
		hitAnimationPrefab = Instantiate(hitAnimation, transform.position, Quaternion.identity) as GameObject;
	}

	public void DeleteDeathPrefab()
	{
		if (hitAnimationPrefab)
		{
			Destroy(hitAnimationPrefab);
			hitAnimationPrefab = null;
		}
	}
}
