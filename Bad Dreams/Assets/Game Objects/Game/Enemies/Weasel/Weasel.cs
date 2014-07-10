using UnityEngine;
using System.Collections;

public class Weasel : MonoBehaviour
{
	const float MAX_SLEEP_TIME =			10.0f;	//sleep times
	const float MIN_SLEEP_TIME =			8.0f;
	const float HEARING_DISTANCE =			3.0f;	//how far the weasel can detect player
	const float MOVING_SPEED =				0.6f;	//alerted/non-alerted moving speed
	const float MIN_PLAYER_DISTANCE =		1.0f;	//how near the weasel gets before attacking
	const float PLAYER_MOVEMENT_DETECT =	0.2f;	//smaller == detect easier
	const float RANDOM_MOVE_RANGE =			4.0f;	//how far do we go when not alerted

	bool alerted;				//is weasel chasing the player
	bool hidden;				//under snow?
	bool seePlayer;				//does the weasel see the player (or hear)
	float alertness;			//!
	float distLeft, distRight; //distance to ledge

	Transform eye;
	Rigidbody2D rigid;
	Vector3 lastPlayerPos, targetPos;
	Transform player;
	Animator atorTail, atorHead;

	//not alerted
	float sleepTime, sleepTimer; //stay in one place and move sometimes a little

	//alerted
	//...

	//debug
	string debugText;

	void Start ()
	{
		rigid = GetComponent<Rigidbody2D>();
		atorTail = GameObject.Find("Tail Graphics").GetComponent<Animator>();
		atorHead = GameObject.Find("Head Graphics").GetComponent<Animator>();
		lastPlayerPos = Vector3.zero;
		seePlayer = false;
		eye = transform.FindChild("Eye Position");
		player = GameObject.Find("Player").transform;
		SwitchToNotAlerted();
	}
	
	void Update ()
	{
		//debug text
		debugText = "";
		debugText += "alert: " + alerted;
		debugText += "\nhidden: " + hidden;
		debugText += "\nseePlayer: " + seePlayer;

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

		//debug text
		GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = debugText;
	}

	void BothBehaviors()
	{
		//test ground from sides
		distRight = 0.0f;
		distLeft = 0.0f;
		LedgeTest(ref distLeft, ref distRight, 0.25f);

		debugText += "\ndistLeft: " + distLeft;
		debugText += "\ndistRight: " + distRight;

		//raycast if player is near
		if (RaycastPlayer(eye.position, new Vector2(1.0f, 0.0f), distRight, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1 || RaycastPlayer(eye.position, new Vector2(-1.0f, 0.0f), -distLeft, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1)
		{
			if (!seePlayer)
			{
				seePlayer = true;
				lastPlayerPos = player.position;
			
				Debug.DrawLine(transform.position, lastPlayerPos, Color.cyan, 1.0f);
				//SwitchToAlerted();
			}
		}
		else
		{
			seePlayer = false;
		}

		PlayerMovementDetection();
	}

	//not alerted
	void SwitchToNotAlerted()
	{
		Debug.Log("weasel: SwitchToNotAlerted");
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
				Debug.Log("random target " + targetPos);
				ResetSleepTimer();
			}
		}

		//random movement
		float dist = Vector2.Distance(transform.position, targetPos);

		if (dist > MIN_PLAYER_DISTANCE)
		{
			if (targetPos.x < transform.position.x)
			{
				rigid.velocity = new Vector2(-MOVING_SPEED, 0.0f);
				atorTail.SetBool("moving", true);
				MirrorSprite(false);

				if (distLeft > -0.6f)
				{
					//stop, target over ledge
					targetPos = transform.position;
					atorTail.SetBool("moving", false);
					rigid.velocity = new Vector2(0.0f, 0.0f);
				}
			}
			else if (targetPos.x > transform.position.x)
			{
				rigid.velocity = new Vector2(MOVING_SPEED, 0.0f);
				atorTail.SetBool("moving", true);
				MirrorSprite(true);

				if (distRight < 0.6f)
				{
					//stop, target over ledge
					targetPos = transform.position;
					atorTail.SetBool("moving", false);
					rigid.velocity = new Vector2(0.0f, 0.0f);
				}
			}
		}
		else
		{
			//stop
			targetPos = transform.position;
			atorTail.SetBool("moving", false);
			rigid.velocity = new Vector2(0.0f, 0.0f);
		}
		
	}

	//alerted
	void SwitchToAlerted()
	{
		Debug.Log("weasel: SwitchToAlerted");
		alerted = true;
	}
	
	void AlertedBehavior()
	{
		float playerDistance = Vector2.Distance(transform.position, lastPlayerPos);

		if (playerDistance > MIN_PLAYER_DISTANCE)
		{
			if (lastPlayerPos.x < transform.position.x)
			{
				rigid.velocity = new Vector2(-MOVING_SPEED, 0.0f);
				atorTail.SetBool("moving", true);
				MirrorSprite(false);
			}
			else if (lastPlayerPos.x > transform.position.x)
			{
				rigid.velocity = new Vector2(MOVING_SPEED, 0.0f);
				atorTail.SetBool("moving", true);
				MirrorSprite(true);
			}
		}
		else
		{
			atorTail.SetBool("moving", false);
			rigid.velocity = new Vector2(0.0f, 0.0f);
			//raise head from snow
			//wait a bit
			//->
			float playerRealDistance = Vector2.Distance(transform.position, player.position);
			if (playerRealDistance > MIN_PLAYER_DISTANCE+0.5f)
			{
				SwitchToNotAlerted();
			}
			else
			{
				atorHead.SetTrigger("show");
				//atorHead.SetTrigger("hide");
				Debug.Log("attack");
			}
			//pop up? if player still there, attaaack!
			//else go non-alert
		}
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
