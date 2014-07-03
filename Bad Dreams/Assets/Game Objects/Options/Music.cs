//
//ADD THIS SCRIPT TO ANY BACKGROUND MUSIC
//

using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{
	void Start () 
    {
        audio.volume = PlayerPrefs.GetFloat("music", 1.0f);

        audio.ignoreListenerVolume = true;
        gameObject.tag = "Music";

        audio.Play();
	}
}
