using UnityEngine;
using System.Collections;

public class Weasel : MonoBehaviour
{
	const float MAX_SLEEP_TIME = 60.0f;
	const float MIN_SLEEP_TIME = 30.0f;
	const float HEARING_DISTANCE = 3.0f;

	bool alerted;	//u mad?
	bool hidden;	//under snow?
	bool seePlayer;	//i see you

	Transform eye;

	//not alerted
	float sleepTime, sleepTimer; //stay in one place and move sometimes a little

	//alerted
	//...

	//debug
	string debugText;

	void Start ()
	{
		seePlayer = false;
		eye = transform.FindChild("Eye Position");
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
		if (alerted)
		{
			AlertedBehavior();
		}
		else
		{
			NotAlertedBehavior();
		}

		//debug text
		GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = debugText;
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
		//test ground from sides
		float testInterval = 0.25f;
		float distRight = 0.0f;
		float distLeft = 0.0f;

		//left
		for (; distLeft > -HEARING_DISTANCE; distLeft -= testInterval)
		{
			if (RaycastTerrain(eye.position + new Vector3(distLeft, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f,1.0f,0.0f,1.0f)) == -1) //no ground, break
			{
				break;
			}
		}
		

		for (; distLeft <= 0; distLeft += 0.025f)
		{
			if (RaycastTerrain(eye.position + new Vector3(distLeft, -0.05f), new Vector2(0.0f, -1.0f), 0.2f, new Color(1.0f,0.5f,0.0f,1.0f)) != -1) //ground, break
			{
				break;
			}
		}
		debugText += "\ndistLeft: " + distLeft;

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
		
		debugText += "\ndistRight: " + distRight;



		//raycast if player is near
		if (RaycastPlayer(eye.position, new Vector2(1.0f, 0.0f), distRight, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1 || RaycastPlayer(eye.position, new Vector2(-1.0f, 0.0f), -distLeft, new Color(1.0f, 0.0f, 0.0f, 1.0f)) != -1)
		{
			seePlayer = true;
		}
		else
		{
			seePlayer = false;
		}

		//sleeping and random movement
		if (sleepTimer < sleepTime)
		{
			sleepTimer += Time.deltaTime;
			if (sleepTimer >= sleepTime)
			{
				//move a little

				ResetSleepTimer();
			}
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
