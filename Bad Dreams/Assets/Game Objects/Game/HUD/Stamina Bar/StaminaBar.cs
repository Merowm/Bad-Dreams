using UnityEngine;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
	GameObject playerObj;
	//Stamina stamina;

	public GameObject[] bars;
	void Start ()
	{
		
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
}
