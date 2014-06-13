using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour
{

	public float regenTime;
	float regenTimer;
	public int staminaMax;
	public int stamina;
	StaminaBar bar;
	bool regenAllow;

	void Start ()
	{
		regenAllow = true;
		regenTimer = 0.0f;
	}
	
	void Update ()
	{
		if (stamina < staminaMax && regenAllow)
		{
			regenTimer += Time.deltaTime;
			if (regenTimer >= regenTime)
			{
				stamina++;
				AddBar(stamina);
				regenTimer = 0.0f;
			}
		}
	}

	public void InitStaminaBar(StaminaBar staminaBarParam)
	{
		bar = staminaBarParam;
	}

	public bool GetRegen()
	{
		return regenAllow;
	}
	public void SetRegen(bool value)
	{
		//Debug.Log("regen " + value);
		regenTimer = 0.0f;
		regenAllow = value;
	}

	void AddBar(int stam)
	{
		if (bar != null)
		{
			bar.AddBar(stam);
		}
	}
	void SubBar(int stam)
	{
		if (bar != null)
		{
			bar.SubBar(stam);
		}
	}

	public bool Remaining()
	{
		return stamina > 0;
	}
	public bool Use()
	{
		if (stamina > 0)
		{
			stamina--;
			SubBar(stamina);
			regenTimer = 0.0f; //?
			return true;
		}
		return false;
	}
}
