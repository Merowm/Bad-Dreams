using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float time;
	float timer;
	UILabel timerLabel;

    //public int TimePassed { get { return (int)(time - timer); } }
    public int TimePassed { get { return (int)timePassed; } set {} }
    private float timePassed;

	void Start ()
	{
		timer = time;
	}
	
	void Update ()
	{
        timePassed += Time.deltaTime;

		if (timerLabel == null)
		{
			timerLabel = GameObject.Find("UI/Timer/Label").GetComponent<UILabel>();
		}
		else
		{
			int mins = (int)(timer / 60);
			int secs = (int)(timer % 60);

			if (secs < 10)
			{
				timerLabel.text = mins + ":0" + secs;
			}
			else
			{
				timerLabel.text = mins + ":" + secs;
			}
			timer -= Time.deltaTime;

			if (timer <= 0.0f)
			{
				GameplayStateManager.SwitchTo(GameplayState.LevelFailed);
			}
		}
	}

	public void Reset()
	{
		timer = time;
        timePassed = 0.0F;
	}

    public void TimeBonus()
    {
        timer = Mathf.Clamp(timer + 20, 0.0F, time);
    }
}
