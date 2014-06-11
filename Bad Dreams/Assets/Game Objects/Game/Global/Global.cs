using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour
{
	int currentScene;
	void Start ()
    {
        GameObject global = GameObject.Find("Global");

		if (global != null)
		{
			Debug.Log("Global found, destroying");
			DestroyImmediate(gameObject);
			return;
		}
		Debug.Log("Global not found, good");
		gameObject.name = "Global";
		DontDestroyOnLoad(gameObject);

		Application.LoadLevel(Application.loadedLevel);
	}
	
	void Update ()
	{
		
	}
}
