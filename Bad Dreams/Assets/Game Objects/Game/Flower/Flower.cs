using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour
{
	public GameObject parentObject;
	public Vector3 offsetFromPlatform;
	void Start ()
	{
	
	}
	
	void Update ()
	{
		if (parentObject)
		{
			transform.position = parentObject.transform.position + offsetFromPlatform;
		}
	}
}
