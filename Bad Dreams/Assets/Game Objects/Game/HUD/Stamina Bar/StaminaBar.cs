using UnityEngine;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
	GameObject playerObj;
	//Stamina stamina;

	public GameObject[] bars;

    private TweenColor[] tweenColors;

	void Start ()
	{
        tweenColors = new TweenColor[bars.Length];
	}
	
	void Update ()
	{
		PlayerAcquired();
	}

	void InitBars()
	{
		for (int i = 0; i < bars.Length; i++)
		{
			bars[i] = GameObject.Find("Bar" + i);
            tweenColors[i] = bars[i].GetComponent<TweenColor>();

			if (bars[i] == null)
			{
				Debug.Log("foobar");
				return;
			}
			else
			{
				//Debug.Log("name " + bars[i].name);
			}
		}
	}

	public void AddBar(int stam)
	{
		//Debug.Log("AddBar " + stam);
		if (bars != null)
		{
			if (stam <= bars.Length)
			{
				if (bars.Length != 0)
				{
					//Debug.Log("add " + (stam - 1));
					bars[stam - 1].SetActive(true);
                    if (tweenColors[stam - 1] != null)
                    {
                        tweenColors[stam - 1].enabled = true;
                        tweenColors[stam - 1].PlayForward();
                    }
				}
			}
		}
	}

	public void SubBar(int stam)
	{
		//Debug.Log("SubBar " + stam);
		if (bars != null)
		{
			if (stam >= 0)
			{
				if (bars.Length != 0)
				{
					//Debug.Log("sub " + (stam + 1));
					bars[stam].SetActive(false);
                    if (tweenColors[stam] != null)
                    {
                        tweenColors[stam].ResetToBeginning();
                    }
				}
			}
		}
	}

	bool PlayerAcquired()
	{
		if (playerObj == null)
		{
			//Debug.Log("looking for player");
			playerObj = GameObject.Find("Player");
			if (playerObj != null)
			{
				//Debug.Log("player found");
				playerObj.GetComponent<Stamina>().InitStaminaBar(this);
				InitBars();
				return true;
			}
			else
			{
				Debug.Log("player not found");
			}
		}
		return true;
	}

    public void ResetStamina()
    {
        if (bars != null)
        {
            foreach (GameObject bar in bars)
            {
                bar.SetActive(true);

            }
        }

        foreach (TweenColor tweenColor in tweenColors)
        {
            tweenColor.PlayForward();
        }
    }
}
