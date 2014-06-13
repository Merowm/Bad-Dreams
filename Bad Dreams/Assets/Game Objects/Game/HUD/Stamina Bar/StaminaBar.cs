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
				Debug.Log("name " + bars[i].name);
			}
		}
	}

	public void AddBar(int stam)
	{
		Debug.Log("AddBar " + stam);
		if (stam <= bars.Length)
		{
			if (bars.Length != 0)
			{
				bars[stam - 1].SetActive(true);
				Debug.Log("add " + (stam - 1));
			}
		}
	}

	public void SubBar(int stam)
	{
		Debug.Log("SubBar " + stam);
		if (stam >= 0)
		{
			if (bars.Length != 0)
			{
				bars[stam].SetActive(false);
				Debug.Log("sub " + (stam + 1));
			}
		}
	}

	bool PlayerAcquired()
	{
		if (playerObj == null)
		{
			Debug.Log("looking for player");
			playerObj = GameObject.Find("Player");
			if (playerObj != null)
			{
				Debug.Log("player found");
				playerObj.GetComponent<Stamina>().InitStaminaBar(this);
				/*stamina = playerObj.GetComponent<Stamina>();
				if (stamina == null)
				{
					Debug.Log("stamina comp not found");
				}*/
				InitBars();
				return true;
			}
		}
		return true;
	}
}
