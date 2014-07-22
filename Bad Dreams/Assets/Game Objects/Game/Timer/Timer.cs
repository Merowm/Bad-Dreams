using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float time;
	public bool freeze, hideTime;
	float timer;
	UILabel timerLabel;

    //public int TimePassed { get { return (int)(time - timer); } }
    public int TimePassed { get { return (int)timePassed; } set {} }
    private float timePassed;

	Animator staminator;
	bool animationTriggered;

	void Start ()
	{
		animationTriggered = false;
		timer = time;
	}

	void StartStaminaAnimation()
	{
		if (staminator && !animationTriggered)
		{
			animationTriggered = true;
			staminator.SetTrigger("startHighlight");
		}
	}

	void ResetStaminaAnimation()
	{
		if (staminator && animationTriggered)
		{
			animationTriggered = false;
			staminator.SetTrigger("reset");
		}
	}

	void Update ()
	{
        timePassed += Time.deltaTime;

		if (!staminator)
		{
			GameObject staminaBG = GameObject.Find("StaminaBG");
			if (staminaBG)
			{
				staminator = staminaBG.GetComponent<Animator>();
			}
		}

		if (timerLabel == null)
		{
			timerLabel = GameObject.Find("UI/Timer/Label").GetComponent<UILabel>();
		}
		if (timerLabel)
		{
			
			if (timer < 21.0f)
			{
				StartStaminaAnimation();
			}
			else
			{
				ResetStaminaAnimation();
			}
			int mins = (int)(timer / 60);
			int secs = (int)(timer % 60);

			if (!hideTime)
			{
				if (secs < 10)
				{
					timerLabel.text = mins + ":0" + secs;
				}
				else
				{
					timerLabel.text = mins + ":" + secs;
				}

				if (timer <= 0.0f)
				{
					timerLabel.text = "0:00";
				}
			}
			else
			{
				timerLabel.text = "";
			}

			if (timer <= 0.0f)
				GameplayStateManager.SwitchTo(GameplayState.LevelFailed);
			
			if (!freeze)
				timer -= Time.deltaTime;
		}
	}

	public void Reset()
	{
		timer = time;
        timePassed = 0.0F;
	}

    private int timeBonusSize = 1;

    public void TimeBonus()
    {
        timePassed -= timeBonusSize;
        timer = Mathf.Clamp(timer + timeBonusSize, 0.0F, time);
    }
}
