using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float time;
	UILabel timerLabel;
	void Start ()
	{
	}
	
	void Update ()
	{
		if (timerLabel == null)
		{
			timerLabel = GameObject.Find("UI/Timer/Label").GetComponent<UILabel>();
		}
		else
		{
			int mins = (int)(time / 60);
			int secs = (int)(time % 60);

			if (secs < 10)
			{
				timerLabel.text = mins + ":0" + secs;
			}
			else
			{
				timerLabel.text = mins + ":" + secs;
			}
			time -= Time.deltaTime;

			if (time <= 0.0f)
			{
				GameplayStateManager.SwitchTo(GameplayState.GameOver);
			}
		}
	}
}
