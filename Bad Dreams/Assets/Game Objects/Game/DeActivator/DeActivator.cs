using UnityEngine;
using System.Collections;

public class DeActivator : MonoBehaviour
{
	public string targetName;

	GameObject targetObj;

	void AcquireTarget()
	{
		if (targetObj == null)
		{
			//Debug.Log("null, acquiring");
			targetObj = GameObject.Find(targetName);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.name == "Player")
		{
			//Debug.Log("player");
			AcquireTarget();
			if (targetObj != null)
			{
				//Debug.Log("found, set active");
				targetObj.SetActive(false);
			}
			/*else
			{
				Debug.Log(name + ": No target found");
			}*/
		}
	}
}
