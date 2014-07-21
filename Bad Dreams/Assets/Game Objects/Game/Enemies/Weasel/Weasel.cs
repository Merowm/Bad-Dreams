using UnityEngine;
using System.Collections;

public class Weasel : MonoBehaviour
{
	const float MAX_SLEEP_TIME =			12.0f;	//sleep times
	const float MIN_SLEEP_TIME =			8.0f;
	const float HEARING_DISTANCE =			6.0f;	//how far the weasel can detect player
	const float MOVING_SPEED =				0.5f;	//non-alerted moving speed
	const float RUNNING_SPEED =				1.55f;	//alerted speed
	const float MIN_PLAYER_DISTANCE =		0.5f;	//how near the weasel gets before attacking
	const float PLAYER_MOVEMENT_DETECT =	0.2f;	//smaller == detect easier
	const float RANDOM_MOVE_RANGE =			4.0f;	//how far do we go when not alerted
	const float ATTACK_SPEED =				0.1f;
	const float ATTACK_RANGE =				1.0f;
	const float NEAR_EDGE_THRESHOLD =		0.75f;	//minimum distance between weasel and ledge

	bool alerted;				//is weasel chasing the player
	bool seePlayer;				//does the weasel see the player (or hear)
	bool attacking;				//

	float alertness;			//!
	float distLeft, distRight;	//distance to ledge
	float attackTimer;			//

	Transform eye, head;
	Rigidbody2D rigid;
	Vector3 lastPlayerPos, targetPos;
	Transform player;
	Animator atorTail, atorHead;

	//not alerted
	float sleepTime, sleepTimer; //stay in one place and move sometimes a little

	//alerted
	//...

	//debug
	bool enableDebug;
	string debugText;

    SoundHandler sh;

	int headShownState, headHiddenState;

	void Start ()
	{
		rigid = GetComponent<Rigidbody2D>();
		atorTail = GameObject.Find("Tail Graphics").GetComponent<Animator>();
		atorHead = GameObject.Find("Head Graphics").GetComponent<Animator>();
		lastPlayerPos = Vector3.zero;
		seePlayer = false;
		eye = transform.FindChild("Eye Position");
		head = transform.FindChild("Head Position");
		player = GameObject.Find("Player").transform;
		SwitchToNotAlerted();
		attackTimer = 0.0f;
		enableDebug = true;

        sh = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();

		headShownState = Animator.StringToHash("Base Layer.WeasleyHeadShown");
		headHiddenState = Animator.StringToHash("Base Layer.WeasleyHeadHidden");
	}
	
	void Update ()
	{
		
		//debug text
		if (enableDebug)
		{
			debugText = "";
			debugText += "alert: " + alerted;
			//debugText += "\nhidden: " + hidden;
			debugText += "\nseePlayer: " + seePlayer;
		}

		//behavior
		BothBehaviors();
		if (alerted)
		{
			AlertedBehavior();
		}
		else
		{
			NotAlertedBehavior();
		}

		//movement

		//debug
		if (enableDebug)
		{
			GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = debugText;

			Debug.DrawLine(transform.position + new Vector3(0.0f, 0.25f),transform.position + new Vector3(HEARING_DISTANCE, 0.25f), Color.magenta);
			Debug.DrawLine(transform.position + new Vector3(0.0f, 0.25f),transform.position + new Vector3(-HEARING_DISTANCE, 0.25f), Color.magenta);
		}
	}

	void BothBehaviors()
	{
		//test ground from sides
		distRight = 0.0f;
		distLeft = 0.0f;
		LedgeTest(ref distLeft, ref distRight, 0.25f);

		if (enableDebug)
		{
			debugText += "\ndistLeft: " + distLeft;
			debugText += "\ndistRight: " + distRight;
		}

		//raycast if player is near
		if (RaycastPlayer(eye.position, new Vector2(1.0f, 0.0f), distRight, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1 || RaycastPlayer(eye.position, new Vector2(-1.0f, 0.0f), -distLeft, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1)
		{
			GameObject playerObj = GameObject.Find("Player");
			if (!playerObj.GetComponent<HidingSkill>().IsHiding)
			{
				if (!seePlayer)
				{
					seePlayer = true;
					lastPlayerPos = player.position;

					if (enableDebug)
						Debug.DrawLine(transform.position, lastPlayerPos, Color.cyan, 1.0f);
					//SwitchToAlerted();
				}
			}
			else
			{
				seePlayer = false;
			}
		}
		else
		{
			seePlayer = false;
		}

		PlayerMovementDetection();
		Attacking();
		Animation();
	}

	void Animation()
	{
		AnimatorStateInfo asi = atorHead.GetCurrentAnimatorStateInfo(0);
		if (asi.nameHash == headHiddenState)
		{
			//atorHead.SetBool("show", false);
			atorHead.SetBool("hide", false);
		}
		else if (asi.nameHash == headShownState)
		{
			atorHead.SetBool("show", false);
			//atorHead.SetBool("hide", false);
		}
	}

	void Attack()
	{
		if (!attacking)
		{
            sh.PlaySound(SoundType.WeaselPop);
			attacking = true;
			attackTimer = 0.0f;
			//atorHead.SetTrigger("show");
			atorHead.SetBool("show",true);
			atorHead.SetBool("hide", false);
		}
	}

	void Attacking()
	{
		if (attacking)
		{
			GameObject playerObj = GameObject.Find("Player");
			attackTimer += Time.deltaTime;
			if (attackTimer >= ATTACK_SPEED)
			{
				attackTimer = 0.0f;
				attacking = false;

				if (Vector3.Distance(player.position, head.position) < ATTACK_RANGE)
				{
					//Debug.Log("BOOM you're ded");
					
					if (!playerObj.GetComponent<HidingSkill>().IsHiding)
					{
						GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
					}
					
				}
				//atorHead.SetTrigger("hide");
				atorHead.SetBool("show", false);
				atorHead.SetBool("hide", true);
				SwitchToNotAlerted();
			}

			float playerDistance = Vector2.Distance(transform.position, player.position);

			if (playerDistance < MIN_PLAYER_DISTANCE && !playerObj.GetComponent<HidingSkill>().IsHiding)
			{
				if (player.position.x < transform.position.x)
				{
					MirrorSprite(false);
				}
				else if (player.position.x > transform.position.x)
				{
					MirrorSprite(true);
				}
			}

			if (enableDebug)
				debugText += "\natt: " + attackTimer;
		}
	}

	//not alerted
	void SwitchToNotAlerted()
	{
		//Debug.Log("weasel: SwitchToNotAlerted");
		alerted = false;
		ResetSleepTimer();
	}

	void NotAlertedBehavior()
	{
		//sleeping and random movement
		if (sleepTimer < sleepTime)
		{
			sleepTimer += Time.deltaTime;
			if (sleepTimer >= sleepTime)
			{
				//move a little
				float rand = Random.value*2.0f - 1.0f;
				targetPos = transform.position + new Vector3(RANDOM_MOVE_RANGE*rand, 0.0f);
				//Debug.Log("random target " + targetPos);
				ResetSleepTimer();
			}
		}

		//random movement
		float dist = Vector2.Distance(transform.position, targetPos);

		if (dist > MIN_PLAYER_DISTANCE)
		{
			if (targetPos.x < transform.position.x)
			{
				if (distLeft > -NEAR_EDGE_THRESHOLD) //target over ledge
				{
					StopMovement();
				}
				else
				{
					MoveLeft(MOVING_SPEED);
				}
			}
			else if (targetPos.x > transform.position.x)
			{
				if (distRight < NEAR_EDGE_THRESHOLD) //target over ledge
				{
					StopMovement();
				}
				else
				{
					MoveRight(MOVING_SPEED);
				}
			}
		}
		else
		{
			StopMovement();
		}

		if (enableDebug)
			debugText += "\nsleep: " + (sleepTime - sleepTimer);
	}

	void MoveLeft(float speedParam)
	{
        sh.PlaySound(SoundType.WeaselMove);
		rigid.velocity = new Vector2(-speedParam, 0.0f);
		atorTail.SetBool("moving", true);
		MirrorSprite(false);
	}

	void MoveRight(float speedParam)
	{
        sh.PlaySound(SoundType.WeaselMove);
		rigid.velocity = new Vector2(speedParam, 0.0f);
		atorTail.SetBool("moving", true);
		MirrorSprite(true);
	}

	void StopMovement()
	{
		//stop
        sh.StopSound(SoundType.WeaselMove);
		targetPos = transform.position;
		atorTail.SetBool("moving", false);
		rigid.velocity = new Vector2(0.0f, 0.0f);
	}

	//alerted
	void SwitchToAlerted()
	{
		//Debug.Log("weasel: SwitchToAlerted");
		alerted = true;
	}
	
	void AlertedBehavior()
	{
		float playerDistance = Vector2.Distance(transform.position, lastPlayerPos);

		if (playerDistance > MIN_PLAYER_DISTANCE)
		{
			if (lastPlayerPos.x < transform.position.x)
			{
				//MoveLeft(MOVING_SPEED);
				if (distLeft > -NEAR_EDGE_THRESHOLD) //target over ledge
				{
					StopMovement();
				}
				else
				{
					MoveLeft(RUNNING_SPEED);
				}
			}
			else if (lastPlayerPos.x > transform.position.x)
			{
				//MoveRight(MOVING_SPEED);
				if (distRight < NEAR_EDGE_THRESHOLD) //target over ledge
				{
					StopMovement();
				}
				else
				{
					MoveRight(RUNNING_SPEED);
				}
			}
		}
		else
		{
			StopMovement();
			Attack();
		}
		if (enableDebug)
			debugText += "\npDist: " + playerDistance;
	}

	void LedgeTest(ref float distLeft, ref float distRight, float testInterval)
	{
		//left
		for (; distLeft > -HEARING_DISTANCE; distLeft -= testInterval)
		{
			if (RaycastTerrain(eye.position + new Vector3(distLeft, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f, 1.0f, 0.0f, 1.0f)) == -1) //no ground, break
			{
				break;
			}
		}


		for (; distLeft <= 0; distLeft += 0.025f)
		{
			if (RaycastTerrain(eye.position + new Vector3(distLeft, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f, 0.5f, 0.0f, 1.0f)) != -1) //ground, break
			{
				break;
			}
		}


		//right
		for (; distRight < HEARING_DISTANCE; distRight += testInterval)
		{
			if (RaycastTerrain(eye.position + new Vector3(distRight, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f, 1.0f, 0.0f, 1.0f)) == -1) //no ground, break
			{
				break;
			}
		}

		for (; distRight >= 0; distRight -= 0.025f)
		{
			if (RaycastTerrain(eye.position + new Vector3(distRight, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f, 0.5f, 0.0f, 1.0f)) != -1) //ground, break
			{
				break;
			}
		}
	}

	void PlayerMovementDetection()
	{
		if (seePlayer)
		{
			float dist = Vector2.Distance(player.position, lastPlayerPos);
			if (dist >= PLAYER_MOVEMENT_DETECT)
			{
				SwitchToAlerted();
				lastPlayerPos = player.position;
			}
			if (enableDebug)
				debugText += "\npMovDetctDst: " + dist;
		}
	}

	void MirrorSprite(bool mirror)
	{
		Vector3 prevScale = transform.localScale;
		float posScale = Mathf.Abs(prevScale.x);
		if (mirror)
		{
			if (prevScale.x > 0.0f)
			{
				prevScale = new Vector3(-posScale, prevScale.y, prevScale.z);
			}
		}
		else
		{
			if (prevScale.x < 0.0f)
			{
				prevScale = new Vector3(posScale, prevScale.y, prevScale.z);
			}
		}
		transform.localScale = prevScale;
	}

	void ResetSleepTimer()
	{
		sleepTime = MIN_SLEEP_TIME + Random.value * (MAX_SLEEP_TIME - MIN_SLEEP_TIME);
		sleepTimer = 0.0f;
	}

	int Raycast(Vector2 pos, Vector2 dir, float len, int layer, Color debugColor) //returns -1 on no collision, otherwise, the layer collided with
	{
		RaycastHit2D hit = Physics2D.Raycast(pos, dir, len, layer);
		if (enableDebug)
			Debug.DrawRay(pos, dir * len, debugColor);

		if (hit != null)
		{
			if (hit.collider != null)
			{
				return hit.collider.gameObject.layer;
			}
		}
		return -1;
	}

	int RaycastPlayer(Vector2 pos, Vector2 dir, float len, Color debugColor)
	{
		//user layer 10 is player
		int layer = (1 << 10);

		return Raycast(pos, dir, len, layer, debugColor);
	}

	int RaycastTerrain(Vector2 pos, Vector2 dir, float len, Color debugColor)
	{
		//user layer 8 is terrain
		int layer = (1 << 8);

		return Raycast(pos, dir, len, layer, debugColor);
	}
}
