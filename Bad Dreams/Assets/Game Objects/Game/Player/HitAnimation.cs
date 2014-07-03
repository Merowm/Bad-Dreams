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
		//GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = "preTransitionTimer " + preTransitionTimer;
		if (active)
		{
			preTransitionTimer += Time.deltaTime;
			if (preTransitionTimer >= timeBeforeTransition)
			{
				StartTransition();
			}
		}
	}

	public void ActivateAnimation() //call this when get hit
	{
		//Debug.Log("ActivateAnimation...");
		if (!active)
		{
			//Debug.Log("ActivateAnimation success");
			active = true;
			player.Kill();
			CreateParticles();
		}
	}

	void StartTransition()
	{
		//Debug.Log("start transition");
		GameplayStateManager.SwitchTo(GameplayState.GameOver);
	}

	public void ResetAnimation()
	{
		if (active)
		{
			active = false;
			preTransitionTimer = 0.0f;
			player.Resurrect();
			DeleteDeathPrefab();
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
