using UnityEngine;
using System.Collections;

public class LethalSpike : MonoBehaviour
{
	bool lowerTrigger, upperTrigger, deathTrigger;

	const float TIME_BETWEEN_LOWER_AND_UPPER = 4.0f;
	float lowerUpperTimer;
	bool lowerUpperTimerEnable;
	void Start ()
	{
		lowerTrigger = false;
		upperTrigger = false;
		deathTrigger = false;
		lowerUpperTimer = TIME_BETWEEN_LOWER_AND_UPPER;
		lowerUpperTimerEnable = true;
	}
	
	void Update ()
	{
		GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = "spikeTimer: " + lowerUpperTimer;
		if (lowerUpperTimerEnable)
		{
			if (lowerUpperTimer <= TIME_BETWEEN_LOWER_AND_UPPER)
			{
				lowerUpperTimer += Time.deltaTime;
			}
		}
	}



	void UpperTriggered()
	{
		if (lowerUpperTimer > TIME_BETWEEN_LOWER_AND_UPPER)
		{
			Debug.Log("UpperTriggered");
			upperTrigger = true;
			lowerTrigger = false;
		}
		else
		{
			Debug.Log("Tried UpperTrigger");
		}
	}

	void DeathTriggered()
	{
		Debug.Log("DeathTriggered");

		if (!lowerTrigger && upperTrigger)
		{
			GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
			lowerTrigger = false;
			upperTrigger = false;
		}
	}

	void LowerTriggered()
	{
		//if (!lowerTrigger)
		{
			Debug.Log("LowerTriggered");
			lowerUpperTimerEnable = false;
			lowerUpperTimer = 0.0f;
			upperTrigger = false;
			lowerTrigger = true;
		}
		/*else
		{
			Debug.Log("Tried LowerTrigger");
		}*/
	}



	void UpperTriggeredExit()
	{
		//Debug.Log("UpperTriggeredExit");
	}

	void DeathTriggeredExit()
	{
		//Debug.Log("DeathTriggeredExit");
	}

	void LowerTriggeredExit()
	{
		lowerUpperTimerEnable = true;
		//upperTrigger = false;
		//lowerTrigger = true;
		//lowerUpperTimerEnable = true;
		//Debug.Log("LowerTriggeredExit");
	}

	void ReceiveTrigger(string trigger)
	{
		switch (trigger)
		{
			case "upper":
			{
				UpperTriggered();
				break;
			}
			case "death":
			{
				DeathTriggered();
				break;
			}
			case "lower":
			{
				LowerTriggered();
				break;
			}
			default:
			{
				Debug.Log("Invalid trigger: " + trigger);
				break;
			}
		}
	}

	void ReceiveTriggerExit(string trigger)
	{
		switch (trigger)
		{
			case "upper":
				{
					UpperTriggeredExit();
					break;
				}
			case "death":
				{
					DeathTriggeredExit();
					break;
				}
			case "lower":
				{
					LowerTriggeredExit();
					break;
				}
			default:
				{
					Debug.Log("Invalid trigger: " + trigger);
					break;
				}
		}
	}
}
