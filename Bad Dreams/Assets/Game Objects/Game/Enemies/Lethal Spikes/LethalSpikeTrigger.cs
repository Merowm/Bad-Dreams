using UnityEngine;
using System.Collections;

public class LethalSpikeTrigger : MonoBehaviour
{
	public string triggerType;
	public GameObject myParent;

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.name == "Player")
		{
			if (myParent)
			{
				myParent.SendMessage("ReceiveTrigger", triggerType);
			}
		}
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if (c.gameObject.name == "Player")
		{
			if (myParent)
			{
				myParent.SendMessage("ReceiveTriggerExit", triggerType);
			}
		}
	}
}
