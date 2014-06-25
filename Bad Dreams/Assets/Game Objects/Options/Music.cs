using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{
	void Start () 
    {
        audio.ignoreListenerVolume = true;
        gameObject.tag = "Music";
	}
}
